using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT06
{
    public class SubDistrictCBB
    {
        public class Data
        {
            public IEnumerable<dynamic> SubDistrict { get; set; }
        }

        public class Query : IRequest<Data>
        {
            public long DistrictId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Data>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<Data> Handle(Query request, CancellationToken cancellationToken)
            {
                var master = new Data() { };
                master.SubDistrict = await this.Get(request, cancellationToken);

                return master;
            }

            private Task<IEnumerable<dynamic>> Get(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(sub_district_code,' : ',get_name(@Lang, sub_district_name_tha, sub_district_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,sub_district_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_sub_district ");
                sql.AppendLine(" WHERE district_id = @DistrictId ");
                sql.AppendLine(" ORDER BY 1  ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new{ Lang = _user.Language, DistrictId = request.DistrictId }, cancellationToken);
            }

        }
    }
}
