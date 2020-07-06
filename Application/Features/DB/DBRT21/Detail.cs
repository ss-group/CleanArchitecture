using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT21
{
    public class Detail
    {
        public class DbFacProgramVm : DbFacProgram
        {
            public string RefGroupId { get; set; }
            public string CurrId { get; set; }
            public string ProgramId { get; set; }
            public string IscedId { get; set; }
            public string ProgramCodeMua { get; set; }
            public new IEnumerable<DbFacProgramDetail> dbFacProgramDetail { get; set; }
        }

        public class DbFacProgramDetailVm : DbFacProgramDetail
        {
        }
        public class Query : IRequest<DbFacProgram>
        {
           
            public string FacCode { get; set; }
            public string ProgramCode { get; set; }

        }
        public class Handler : IRequestHandler<Query, DbFacProgram>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbFacProgram> Handle(Query request, CancellationToken cancellationToken)
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT dfp.company_code AS \"companyCode\" ");
                sql.AppendLine(" 	, dfp.fac_code AS \"facCode\" ");
                sql.AppendLine(" 	, dfp.program_code AS \"programCode\" ");
                sql.AppendLine(" 	, p.ref_group_id AS \"refGroupId\" ");
                sql.AppendLine("  	, p.curr_id AS \"currId\" ");
                sql.AppendLine("  	, p.program_id AS \"programId\" ");
                sql.AppendLine("  	, p.isced_id AS \"iscedId\" ");
                sql.AppendLine("  	, p.program_code_mua AS \"programCodeMua\" ");
                sql.AppendLine(" 	, dfp.active ");
                sql.AppendLine(" 	, dfp.xmin as \"rowVersion\" ");
                sql.AppendLine(" FROM db_fac_program dfp ");
                sql.AppendLine(" 	JOIN db_program p ");
                sql.AppendLine(" 		ON p.company_code = dfp.company_code ");
                sql.AppendLine("  		AND p.program_code = dfp.program_code ");
                sql.AppendLine(" WHERE dfp.program_code = @ProgramCode ");
                sql.AppendLine(" 	AND dfp.company_code = @CompanyCode ");
                sql.AppendLine(" 	AND dfp.fac_code = @FacCode ");

                var subj = await _context.QueryFirstOrDefaultAsync<DbFacProgramVm>(sql.ToString()
                    , new { CompanyCode = _user.Company, FacCode = request.FacCode, ProgramCode = request.ProgramCode }, cancellationToken);

                StringBuilder sqlSub = new StringBuilder();
                sqlSub.AppendLine("SELECT dfpd.company_code AS \"companyCode\" ");
                sqlSub.AppendLine(", dfpd.fac_code AS \"facCode\", dfpd.program_code AS \"programCode\", dfpd.department_code AS \"departmentCode\" ");
                sqlSub.AppendLine(", dfpd.active, dfpd.xmin as \"rowVersion\" ");
                sqlSub.AppendLine("FROM db_fac_program_detail dfpd ");
                sqlSub.AppendLine("WHERE dfpd.program_code = @ProgramCode ");
                sqlSub.AppendLine("AND dfpd.company_code = @CompanyCode ");
                sqlSub.AppendLine("AND dfpd.fac_code = @FacCode ");

                subj.dbFacProgramDetail = await _context.QueryAsync<DbFacProgramDetailVm>(sqlSub.ToString()
                    , new { CompanyCode = _user.Company, FacCode = request.FacCode, ProgramCode = request.ProgramCode }, cancellationToken);

                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
