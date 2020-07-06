using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT06
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
                sql.AppendLine("SELECT pc.postal_code_id AS \"postalCodeId\" ");
                sql.AppendLine("  ,pc.postal_code AS \"postalCode\" ");
                sql.AppendLine("  ,pc.postal_name_tha AS \"postalNameTha\" ");
                sql.AppendLine("  ,pc.postal_name_eng AS \"postalNameEng\" ");
                sql.AppendLine("  ,get_name(@Lang,sd.sub_district_name_tha,sd.sub_district_name_eng) AS \"subDistrictId\" ");
                sql.AppendLine("  ,get_name(@Lang,d.district_name_tha,d.district_name_eng) AS \"districtId\" ");
                sql.AppendLine("  ,get_name(@Lang,p.province_name_tha,p.province_name_eng) AS \"provinceId\" ");
                sql.AppendLine("  ,pc.active AS \"active\" ");
                sql.AppendLine("  ,pc.xmin AS \"rowVersion\" ");
                sql.AppendLine("FROM db_postal_code pc");
                sql.AppendLine("JOIN db_sub_district sd on sd.sub_district_id = pc.sub_district_id");
                sql.AppendLine("JOIN db_district d on d.district_id = pc.district_id");
                sql.AppendLine("JOIN db_province p on p.province_id = pc.province_id");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(postal_code,postal_name_tha,postal_name_eng,get_name(@Lang,sd.sub_district_name_tha,sd.sub_district_name_eng),get_name(@Lang,d.district_name_tha,d.district_name_eng),get_name(@Lang,p.province_name_tha,p.province_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Lang = _user.Language}, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
