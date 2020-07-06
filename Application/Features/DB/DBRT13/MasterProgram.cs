using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT13
{
    public class MasterProgram
    {
        public class MasterData
        {
            public IEnumerable<dynamic> ProgramCode { get; set; }
        }

        public class Query : IRequest<MasterData>
        {
            public string FacCode { get; set; }
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
                var programCode = await this.GetProgramCode(request, cancellationToken);

                return new MasterData { ProgramCode = programCode };
            }

            private Task<IEnumerable<dynamic>> GetProgramCode(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(p.program_code, ' : ',get_name(@lang, p.program_name_tha, p.program_name_eng)) AS text, p.program_code AS value");
                sql.AppendLine(", fp.active AS active ");
                sql.AppendLine("FROM db_fac_program fp ");
                sql.AppendLine("JOIN db_program p ON (p.company_code = fp.company_code AND p.program_code = fp.program_code) ");
                sql.AppendLine("WHERE fp.company_code = @CompanyCode ");
                sql.AppendLine("AND fp.fac_code = @FacCode ");
                sql.AppendLine("ORDER BY 1 ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language, CompanyCode = _user.Company
                    , FacCode = request.FacCode }, cancellationToken);
            }
        }
    }
}
