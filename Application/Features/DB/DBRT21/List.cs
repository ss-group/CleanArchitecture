using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT21
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
                sql.AppendLine(" SELECT f.fac_code AS \"facCode\" ");
                sql.AppendLine(" 	, d.department_code AS \"departmentCode\" ");
                sql.AppendLine(" 	, p.program_code AS \"programCode\" ");
                sql.AppendLine(" 	, get_name(@lang, f.fac_name_tha, f.fac_name_eng) AS \"facName\" ");
                sql.AppendLine(" 	, get_name(@lang, d.department_name_tha, d.department_name_eng ) AS \"departmentName\" ");
                sql.AppendLine(" 	, p.program_name_tha AS \"programNameTha\" ");
                sql.AppendLine(" 	, p.program_name_eng AS \"programNameEng\" ");
                sql.AppendLine(" 	, p.ref_group_id AS \"refGroupId\" ");
                sql.AppendLine(" 	, p.curr_id AS \"currId\" ");
                sql.AppendLine(" 	, p.program_id AS \"programId\" ");
                sql.AppendLine(" 	, p.isced_id AS \"iscedId\" ");
                sql.AppendLine(" 	, p.program_code_mua AS \"programCodeMua\" ");
                sql.AppendLine(" 	, fp.active ");
                sql.AppendLine(" 	, fp.xmin AS \"rowVersion\" ");
                sql.AppendLine(" FROM db_fac_program fp ");
                sql.AppendLine(" 	JOIN db_fac f ");
                sql.AppendLine(" 		ON f.company_code = fp.company_code ");
                sql.AppendLine(" 		AND f.fac_code = fp.fac_code ");
                sql.AppendLine(" 	JOIN db_program p ");
                sql.AppendLine(" 		ON p.company_code = fp.company_code ");
                sql.AppendLine(" 		AND p.program_code = fp.program_code ");
                sql.AppendLine(" 	LEFT JOIN db_fac_program_detail fpd ");
                sql.AppendLine(" 		ON fpd.fac_code = fp.fac_code ");
                sql.AppendLine(" 		AND fpd.program_code = fp.program_code  ");
                sql.AppendLine(" 		AND fpd.company_code = fp.company_code  ");
                sql.AppendLine(" 	LEFT JOIN db_department d ");
                sql.AppendLine(" 		ON d.company_code = fpd.company_code ");
                sql.AppendLine(" 		AND d.department_code = fpd.department_code ");
                sql.AppendLine(" WHERE fp.company_code = @companyCode ");
                if (!string.IsNullOrWhiteSpace(request.Keyword)) 
                {
                    sql.AppendLine(" 	AND CONCAT( get_name(@lang, f.fac_name_tha, f.fac_name_eng), get_name(@lang, p.program_name_tha, p.program_name_eng) ) ILIKE '%' || COALESCE(@keyword, '') || '%' ");
                }
                sql.AppendLine(" ORDER BY f.fac_code, d.department_code, p.program_code ");

                return await _context.GetPage(sql.ToString(), new { companyCode = _user.Company, keyword = request.Keyword, lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
