using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT05
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
                sql.AppendLine("SELECT sd.sub_district_id AS \"subDistrictId\" ");
                sql.AppendLine("  ,sd.province_id AS \"provinceId\" ");
                sql.AppendLine("  ,sd.sub_district_code AS \"subDistrictCode\" ");
                sql.AppendLine("  ,sd.sub_district_code_mua AS \"subDistrictCodeMua\" ");
                sql.AppendLine("  ,sd.sub_district_name_tha AS \"subDistrictNameTha\" ");
                sql.AppendLine("  ,sd.sub_district_name_eng AS \"subDistrictNameEng\" ");
                sql.AppendLine("  ,get_name(@Lang,d.district_name_tha, d.district_name_eng) AS \"districtId\" ");
                sql.AppendLine("  ,sd.active AS \"active\" ");
                sql.AppendLine("  ,sd.xmin AS \"rowVersion\" ");
                sql.AppendLine("FROM db_sub_district sd");
                sql.AppendLine("JOIN db_district d on sd.district_id = d.district_id");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(sub_district_code, sub_district_code_mua, sub_district_name_tha, sub_district_name_eng, get_name(@Lang,district_name_tha,district_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
