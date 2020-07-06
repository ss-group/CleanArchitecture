using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT13
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string CompanyCode { get; set; }
            public string Keyword { get; set; }
        }
        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT m.major_code AS \"majorCode\" ");
                sql.AppendLine(" ,m.major_code_mua AS \"majorCodeMua\" ");
                sql.AppendLine(" ,m.fac_code AS \"facCode\" ");
                sql.AppendLine(" ,m.program_code AS \"programCode\" ");
                sql.AppendLine(" ,m.major_name_tha AS \"majorNameTha\" ");
                sql.AppendLine(" ,m.major_name_eng AS \"majorNameEng\" ");
                sql.AppendLine(" ,get_name(@Lang, f.fac_name_tha, f.fac_name_eng) AS \"facName\" ");
                sql.AppendLine(" ,get_name(@Lang, p.program_name_tha, p.program_name_eng) AS \"programName\" ");
                sql.AppendLine(" ,m.active AS \"active\" ");
                sql.AppendLine(" ,m.xmin AS \"rowVersion\" ");
                sql.AppendLine(" FROM db_major m ");
                sql.AppendLine(" JOIN db_fac f ON f.fac_code = m.fac_code ");
                sql.AppendLine(" JOIN db_program p ON  p.program_code = m.program_code ");
                sql.AppendLine(" WHERE m.company_code = @Company ");
                sql.AppendLine(" AND f.company_code = @Company ");
                sql.AppendLine(" AND p.company_code = @Company ");
                if (!string.IsNullOrWhiteSpace(request.Keyword)) 
                {
                    sql.AppendLine("AND CONCAT(major_code, major_code_mua, major_name_tha, major_name_eng, get_name(@Lang, f.fac_name_tha, f.fac_name_eng), get_name(@Lang, p.program_name_tha, p.program_name_eng) ) ");
                    sql.AppendLine(" ILIKE '%' || @Keyword || '%' ");
                }
                sql.AppendLine("ORDER BY m.major_code, m.fac_code, m.program_code, get_name(@Lang, f.fac_name_tha, f.fac_name_eng) ,get_name(@Lang, p.program_name_tha, p.program_name_eng) ");

                return await _context.GetPage(sql.ToString(), new { lang = _user.Language, Company = _user.Company, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
