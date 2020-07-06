using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
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
                var typeLevel = await this.GetEducation(cancellationToken);

                return new MasterData { level = typeLevel };
            }

            private Task<IEnumerable<dynamic>> GetEducation(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(education_type_level,' : ',get_name(@lang, education_type_level_name_tha, education_type_level_name_eng)) AS text, education_type_level AS value");
                sql.AppendLine(", active AS active ");
                sql.AppendLine("FROM db_education_type_level ");
                sql.AppendLine("WHERE company_code = @CompanyCode");
                sql.AppendLine("ORDER BY 1 ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { CompanyCode = _user.Company, lang = _user.Language }, cancellationToken);
            }
        }
    }
}
