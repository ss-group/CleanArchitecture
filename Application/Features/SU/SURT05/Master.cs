using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT05
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> ParameterGroupCodes { get; set; }
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
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT      parameter_group_code AS text,");
                sql.AppendLine("            parameter_group_code AS value");
                sql.AppendLine("FROM        su_parameter");
                sql.AppendLine("GROUP BY    parameter_group_code");
                sql.AppendLine("ORDER BY    parameter_group_code");
                var parameterGroupCodes = await _context.QueryAsync<dynamic>(sql.ToString(), null, cancellationToken);
                
                return new MasterData { ParameterGroupCodes = parameterGroupCodes };
            }
        }
    }
}
