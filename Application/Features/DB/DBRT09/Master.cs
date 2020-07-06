using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT09
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> level { get; set; }
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
                var groupLevel = await this.GetEducation(cancellationToken);

                return new MasterData { level = groupLevel };
            }

            private Task<IEnumerable<dynamic>> GetEducation(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(list_item_code,' : ',get_name(@lang, list_item_name_tha, list_item_name_eng)) AS text, list_item_code AS value");
                sql.AppendLine(", active AS active ");
                sql.AppendLine("FROM db_list_item ");
                sql.AppendLine("WHERE list_item_group_code = 'EduGroupLevel' ");
                sql.AppendLine("ORDER BY sequence");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language }, cancellationToken);
            }
        }
    }
}
