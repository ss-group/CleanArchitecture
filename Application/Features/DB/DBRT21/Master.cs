using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT21
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> FacCode { get; set; }
            public IEnumerable<dynamic> ProgramCode { get; set; }
            public IEnumerable<dynamic> DepartmentCode { get; set; }
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
                var facCode = await this.GetFacCode(cancellationToken);
                var programCode = await this.GetProgramCode(request, cancellationToken);
                var departmentCode = await this.GetDepartmentCode(cancellationToken);
                

                return new MasterData { FacCode = facCode, ProgramCode = programCode, DepartmentCode = departmentCode };
            }

            private Task<IEnumerable<dynamic>> GetFacCode(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(fac_code,' : ',get_name(@lang, fac_name_tha, fac_name_eng)) AS text, fac_code AS value");
                sql.AppendLine(", active AS active ");
                sql.AppendLine("FROM db_fac ");
                sql.AppendLine("WHERE company_code = @CompanyCode ");
                sql.AppendLine("ORDER BY fac_code::numeric ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language, CompanyCode = _user.Company }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetProgramCode(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(program_code,' : ',get_name(@lang, program_name_tha, program_name_eng)) AS text, program_code AS value");
                sql.AppendLine("    , active AS active ");
                sql.AppendLine("	, ref_group_id AS \"refGroupId\" ");
                sql.AppendLine("  	, curr_id AS \"currId\" ");
                sql.AppendLine("  	, program_id AS \"programId\" ");
                sql.AppendLine("  	, isced_id AS \"iscedId\" ");
                sql.AppendLine("  	, program_code_mua AS \"programCodeMua\" ");
                sql.AppendLine("FROM db_program ");
                sql.AppendLine("WHERE company_code = @CompanyCode ");
                sql.AppendLine("ORDER BY program_code::numeric");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language, CompanyCode = _user.Company }, cancellationToken);
            }

            private Task<IEnumerable<dynamic>> GetDepartmentCode(CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT CONCAT(department_code,' : ',get_name(@lang, department_name_tha, department_name_eng)) AS text, department_code AS value");
                sql.AppendLine(", active AS active ");
                sql.AppendLine("FROM db_department ");
                sql.AppendLine("WHERE company_code = @CompanyCode ");
                sql.AppendLine("ORDER BY department_code::numeric ");
                return _context.QueryAsync<dynamic>(sql.ToString(), new { lang = _user.Language, CompanyCode = _user.Company }, cancellationToken);
            }
        }
    }
}
