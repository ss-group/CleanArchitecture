using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT02
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Country { get; set; }
        }

        public class Query : IRequest<MasterData>
        {

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
                var country = await this.GetCountry(cancellationToken);

                return new MasterData { Country = country };
            }

            private Task<IEnumerable<dynamic>> GetCountry(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT CONCAT(country_code,' : ',get_name(@Lang,country_name_tha,country_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,country_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine("FROM db_country ");
                sql.AppendLine("ORDER BY 1");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
        }
    }
}
