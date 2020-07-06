using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT11
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
            public string CompanyCode { get; set; }
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
                sql.AppendLine("SELECT fac_code as \"facCode\" ");
                sql.AppendLine(",fn_payin_code as \"fnPayinCode\", format_code as \"formatCode\" ");
                sql.AppendLine(",fac_name_tha as \"facNameTha\",fac_name_eng as \"facNameEng\" ");
                sql.AppendLine(",fac_short_name_tha \"facShortNameTha\",fac_short_name_eng \"facShortNameEng\" ");
                sql.AppendLine(",fac_code_mua as \"facCodeMua\" ");
                sql.AppendLine(",active,xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_fac");
                sql.AppendLine("WHERE company_code = @Company");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(fac_code,' ',fac_code_mua,' ',fn_payin_code,' ',format_code,' ',fac_name_tha,fac_name_eng,' ',fac_name_eng,fac_short_name_tha,' ',fac_short_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Company = _user.Company, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
