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

namespace Application.Features.DB.DBRT13
{
    public class Detail
    {
        public class DbMajorVm : DbMajor
        {
            public new IEnumerable<DbProfessional> dbProfessional { get; set; }
        }

        public class DbProfessionalVm : DbProfessional
        {
        }
        public class Query : IRequest<DbMajor>
        {
            public string ProgramCode { get; set; }
            public string FacCode { get; set; }
            public string MajorCode { get; set; }
            /*public string CompanyCode { get; set; }*/

        }
        public class Handler : IRequestHandler<Query, DbMajor>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbMajor> Handle(Query request, CancellationToken cancellationToken)
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT dmj.company_code AS \"companyCode\", dmj.major_code AS \"majorCode\" ");
                sql.AppendLine(", dmj.fac_code AS \"facCode\", dmj.program_code AS \"programCode\" ");
                sql.AppendLine(", dmj.major_name_tha AS \"majorNameTha\", dmj.major_name_eng AS \"majorNameEng\" ");
                sql.AppendLine(", dmj.major_short_name_tha AS \"majorShortNameTha\", dmj.major_short_name_eng AS \"majorShortNameEng\" ");
                sql.AppendLine(", dmj.format_code AS \"formatCode\", dmj.major_code_mua AS \"majorCodeMua\" ");
                sql.AppendLine(", dmj.active, dmj.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_major dmj ");
                sql.AppendLine("WHERE dmj.program_code = @ProgramCode ");
                sql.AppendLine("AND dmj.company_code = @CompanyCode ");
                sql.AppendLine("AND dmj.major_code = @MajorCode ");
                sql.AppendLine("AND dmj.fac_code = @FacCode ");

                var subj = await _context.QueryFirstOrDefaultAsync<DbMajorVm>(sql.ToString()
                    , new { CompanyCode = _user.Company, ProgramCode = request.ProgramCode, MajorCode = request.MajorCode
                    , FacCode = request.FacCode}, cancellationToken);

                StringBuilder sqlSub = new StringBuilder();
                sqlSub.AppendLine("SELECT dpf.company_code AS \"companyCode\", dpf.major_code AS \"majorCode\" ");
                sqlSub.AppendLine(", dpf.fac_code AS \"facCode\", dpf.program_code AS \"programCode\" ");
                sqlSub.AppendLine(", dpf.pro_code AS \"proCode\", dpf.pro_name_tha AS \"proNameTha\" ");
                sqlSub.AppendLine(", dpf.pro_name_eng AS \"proNameEng\", dpf.pro_short_name_tha AS \"proShortNameTha\" ");
                sqlSub.AppendLine(", dpf.pro_short_name_eng AS \"proShortNameEng\", dpf.format_code AS \"formatCode\" ");
                sqlSub.AppendLine(", dpf.active, dpf.xmin as \"rowVersion\" ");
                sqlSub.AppendLine("FROM db_professional dpf ");
                sqlSub.AppendLine("WHERE dpf.program_code = @ProgramCode ");
                sqlSub.AppendLine("AND dpf.company_code = @CompanyCode ");
                sqlSub.AppendLine("AND dpf.major_code = @MajorCode ");
                sqlSub.AppendLine("AND dpf.fac_code = @FacCode ");
                sqlSub.AppendLine("ORDER BY dpf.major_code, dpf.fac_code, dpf.program_code ");


                subj.dbProfessional = await _context.QueryAsync<DbProfessionalVm>(sqlSub.ToString()
                    , new { CompanyCode = _user.Company, ProgramCode = request.ProgramCode
                    , MajorCode = request.MajorCode, FacCode = request.FacCode}, cancellationToken);

                if (subj == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return subj;
            }
        }
    }
}
