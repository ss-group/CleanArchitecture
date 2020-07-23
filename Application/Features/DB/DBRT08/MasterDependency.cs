using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class MasterDependency
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Districts { get; set; }
            public IEnumerable<dynamic> SubDistricts { get; set; }
            public IEnumerable<dynamic> PostalCodes { get; set; }

        }

        public class Query : IRequest<MasterData>
        {
            public string Name { get; set; }
            public int ProvinceId { get; set; }
            public int DistrictId { get; set; }
            public int SubDistrictId { get; set; }
        }

        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                var master = new MasterData() { };
                switch (request.Name)
                {
                    case "district":
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine(" SELECT CONCAT(district_code,' : ',get_name(@Lang, district_name_tha, district_name_eng)) AS text ");
                        sql.AppendLine(" ,district_id AS value ");
                        sql.AppendLine(" ,active AS active");
                        sql.AppendLine(" FROM db_district ");
                        sql.AppendLine(" WHERE province_id = @ProvinceId ");
                        sql.AppendLine(" ORDER BY length(district_code),district_code");
                        var districts = await _context.QueryAsync<dynamic>(sql.ToString(), new { ProvinceId = request.ProvinceId, Lang = _user.Language }, cancellationToken);

                        master = new MasterData { Districts = districts };
                        break;
                    case "subDistrict":
                        StringBuilder sqlSub = new StringBuilder();
                        sqlSub.AppendLine("SELECT CONCAT(sub_district_code,' : ',get_name(@Lang, sub_district_name_tha, sub_district_name_eng)) AS text");
                        sqlSub.AppendLine(" ,sub_district_id AS value");
                        sqlSub.AppendLine(" ,active AS active ");
                        sqlSub.AppendLine(" FROM db_sub_district ");
                        sqlSub.AppendLine(" WHERE district_id = @DistrictId ");
                        sqlSub.AppendLine(" ORDER BY length(sub_district_code),sub_district_code ");
                        var subDistricts = await _context.QueryAsync<dynamic>(sqlSub.ToString(), new { DistrictId = request.DistrictId, Lang = _user.Language }, cancellationToken);

                        master = new MasterData { SubDistricts = subDistricts };
                        break;
                    case "postalCode":
                        StringBuilder sqlPost = new StringBuilder();
                        sqlPost.AppendLine(" SELECT CONCAT(postal_code,' : ',get_name(@Lang, postal_name_tha, postal_name_eng)) AS \"text\" ");
                        sqlPost.AppendLine(" ,postal_code_id AS \"value\" ");
                        sqlPost.AppendLine(" ,active AS \"active\" ");
                        sqlPost.AppendLine(" FROM db_postal_code ");
                        sqlPost.AppendLine(" WHERE sub_district_id = @SubDistrictId ");
                        sqlPost.AppendLine(" ORDER BY length(postal_code),postal_code ");
                        var postalCodes = await _context.QueryAsync<dynamic>(sqlPost.ToString(), new { Lang = _user.Language, SubDistrictId = request.SubDistrictId }, cancellationToken);
                        master = new MasterData { PostalCodes = postalCodes };
                        break;
                }
                return master;
            }

        }
    }
}
