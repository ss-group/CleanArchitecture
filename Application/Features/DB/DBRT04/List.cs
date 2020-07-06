using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT04
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
                sql.AppendLine("SELECT d.district_id AS \"districtId\" ");
                sql.AppendLine(" ,d.district_code AS \"districtCode\" ");
                sql.AppendLine(" ,d.district_code_mua AS \"districtCodeMua\" ");
                sql.AppendLine(" ,d.district_name_tha AS \"districtNameTha\" ");
                sql.AppendLine(" ,d.district_name_eng AS \"districtNameEng\" ");
                sql.AppendLine(" ,get_name(@Lang, p.province_name_tha, p.province_name_eng) AS \"provinceId\" ");
                sql.AppendLine(" ,d.active AS \"active\" ");
                sql.AppendLine(" ,d.xmin AS \"rowVersion\" ");
                sql.AppendLine("FROM db_district d");
                sql.AppendLine("JOIN db_province p on d.province_id = p.province_id");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(district_code,' ',district_code_mua,' ',district_name_tha,' ',district_name_eng,' ',get_name(@Lang, p.province_name_tha, p.province_name_eng)) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }


        }
    }
}
