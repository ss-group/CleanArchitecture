using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT01
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
        }
        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT country_id as \"countryId\" ");
                sql.AppendLine(" ,country_code as \"countryCode\" ");
                sql.AppendLine(" ,country_code_mua as \"countryCodeMua\" ");
                sql.AppendLine(" ,country_name_tha as \"countryNameTha\" ");
                sql.AppendLine(" ,country_name_eng as \"countryNameEng\" ");
                sql.AppendLine(" ,country_short_name_tha as \"countryShortNameTha\" ");
                sql.AppendLine(" ,country_short_name_eng as \"countryShortNameEng\" ");
                sql.AppendLine(" ,active as \"active\" ");
                sql.AppendLine(" ,xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_country");
              
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(country_code,country_code_mua,country_name_tha,country_name_eng,country_short_name_tha,country_short_name_eng) ILIKE '%' || @Keyword || '%'");
               
                sql.AppendLine("ORDER BY length(country_code),country_code");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
