using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT02
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
                sql.AppendLine(" SELECT region_id as \"regionId\" ");
                sql.AppendLine(" ,get_name(@Lang,c.country_name_tha,c.country_name_eng) as \"countryName\" ");
                sql.AppendLine(" ,region_code as \"regionCode\" ");
                sql.AppendLine(" ,region_code_mua as \"regionCodeMua\" ");
                sql.AppendLine(" ,region_name_tha as \"regionNameTha\" ");
                sql.AppendLine(" ,region_name_eng as \"regionNameEng\" ");
                sql.AppendLine(" ,region_short_name_tha as \"regionShortNameTha\" ");
                sql.AppendLine(" ,region_short_name_eng as \"regionShortNameEng\" ");
                sql.AppendLine(" ,r.active as \"active\" ");
                sql.AppendLine(" ,r.xmin as \"rowVersion\" ");
                sql.AppendLine(" FROM db_region r");
                sql.AppendLine(" JOIN db_country c on r.country_id = c.country_id");
                sql.AppendLine(" ORDER BY length(region_code),region_code");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(region_code,' ',region_code_mua,' ',region_name_tha,' ',region_name_eng,' ',region_short_name_tha,' ',region_short_name_eng,' ',get_name(@Lang,c.country_name_tha,c.country_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
