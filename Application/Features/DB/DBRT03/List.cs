using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT03
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
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT p.province_id as \"provinceId\" ");
                sql.AppendLine(" ,p.province_code as \"provinceCode\" ");
                sql.AppendLine(" ,p.province_code_mua as \"provinceCodeMua\" ");
                sql.AppendLine(" ,p.province_name_tha as \"provinceNameTha\" ");
                sql.AppendLine(" ,p.province_name_eng as \"provinceNameEng\" ");
                sql.AppendLine(" ,get_name(@Lang,c.country_name_tha,c.country_name_eng) as \"countryId\" ");
                sql.AppendLine(" ,p.active as \"active\" ");
                sql.AppendLine(" ,p.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_province p");
                sql.AppendLine("JOIN db_country c on p.country_id = c.country_id");
                sql.AppendLine("ORDER BY length(province_code),province_code");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(province_code,' ',province_code_mua,' ',province_name_tha,' ',province_name_eng,' ',get_name(@Lang,c.country_name_tha,c.country_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
