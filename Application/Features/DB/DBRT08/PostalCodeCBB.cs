using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class PostalCodeCBB
    {
        public class Data
        {
            public IEnumerable<dynamic> PostalCode { get; set; }
        }

        public class Query : IRequest<Data>
        {
            public long SubDistrictId { get; set; }
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
                master.PostalCode = await this.Get(request, cancellationToken);

                return master;
            }

            private Task<IEnumerable<dynamic>> Get(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(postal_code,' : ',get_name(@Lang, postal_name_tha, postal_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,postal_code_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine(" FROM db_postal_code ");
                sql.AppendLine(" WHERE sub_district_id = @SubDistrictId ");
                sql.AppendLine(" ORDER BY 1 ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new{ Lang = _user.Language, SubDistrictId = request.SubDistrictId}, cancellationToken);
            }

        }
    }
}
