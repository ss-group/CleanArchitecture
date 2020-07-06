using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT04
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Province { get; set; }
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
                var province = await this.GetProvince(cancellationToken);

                return new MasterData { Province = province };
            }
            private Task<IEnumerable<dynamic>> GetProvince(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(province_code,' : ',get_name(@Lang, province_name_tha, province_name_eng)) AS \"text\" ");
                sql.AppendLine(" ,province_id AS \"value\" ");
                sql.AppendLine(" ,active AS \"active\" ");
                sql.AppendLine("FROM db_province ");
                sql.AppendLine("ORDER BY province_code::numeric");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
        }
    }
}
