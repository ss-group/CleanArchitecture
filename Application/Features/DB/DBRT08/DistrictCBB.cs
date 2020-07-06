using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class DistrictCBB
    {
        public class Data
        {
            public IEnumerable<dynamic> District { get; set; }
        }

        public class Query : IRequest<Data>
        {
            public long ProvinceId { get; set; }
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
                master.District = await this.Get(request, cancellationToken);

                return master;
            }

            private Task<IEnumerable<dynamic>> Get(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(district_code,' : ',get_name(@Lang, district_name_tha, district_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,district_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_district ");
                sql.AppendLine(" WHERE province_id = @ProvinceId ");
                sql.AppendLine(" ORDER BY 1  ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new{Lang = _user.Language, ProvinceId = request.ProvinceId }, cancellationToken);
            }

        }
    }
}
