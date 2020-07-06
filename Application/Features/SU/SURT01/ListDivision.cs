using Application.Common.Interfaces;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class ListDivision
    {
        public class Division
        {
            public Company Company { get; set; }
            public PageDto Divisions { get; set; }
        }

        public class Company
        {           
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
        }

        public class Query : RequestPageQuery, IRequest<Division>
        {
            public string Keyword { get; set; }
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, Division>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Division> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select      company_code as \"companyCode\",");
                sql.AppendLine("            case @Language when 'th' then company_name_tha else coalesce(company_name_eng, company_name_tha) end as \"companyName\",");
                sql.AppendLine("            xmin as \"rowVersion\"");
                sql.AppendLine("from        su_company");
                sql.AppendLine("where       company_code = @CompanyCode");
                var company = await _context.QueryFirstOrDefaultAsync<Company>(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine("select      div1.company_code as \"companyCode\",");
                sql.AppendLine("            div1.div_code as \"divCode\",");
                sql.AppendLine("            div1.div_name_tha as \"divNameTha\",");
                sql.AppendLine("            div1.div_name_eng as \"divNameEng\",");
                sql.AppendLine("            case @Language when 'th' then div2.div_name_tha else coalesce(div2.div_name_eng, div2.div_name_tha) end as \"divParent\",");
                sql.AppendLine("            case status.status_value when 'N'");
                sql.AppendLine("            						 then null");
                sql.AppendLine("            						 else case @Language when 'th' then status.status_desc_tha else coalesce(status.status_desc_eng, status.status_desc_tha) end");
                sql.AppendLine("            						 end as \"divType\",");
                sql.AppendLine("            case div1.div_type when 'L' then concat(locat.location_code, ' : ', case @Language when 'th' then locat.location_name_tha else coalesce(locat.location_name_eng, locat.location_name_tha) end)");
                sql.AppendLine("            				   when 'F' then concat(fac.fac_code, ' : ', case @Language when 'th' then fac.fac_name_tha else coalesce(fac.fac_name_eng, fac.fac_name_tha) end)");
                sql.AppendLine("                               when 'P' then concat(program.program_code, ' : ', case @Language when 'th' then program.program_name_tha else coalesce(program.program_name_eng, program.program_name_tha) end, ' ', fac.fac_code, ' : ', case @Language when 'th' then fac.fac_name_tha else coalesce(fac.fac_name_eng, fac.fac_name_tha) end)");
                sql.AppendLine("            				   else null end as \"divTypeDetail\",");
                sql.AppendLine("            div1.xmin as \"rowVersion\"");
                sql.AppendLine("from        su_division as div1");
                sql.AppendLine("left join   su_division as div2");
                sql.AppendLine("on          div1.company_code = div2.company_code");
                sql.AppendLine("            and div1.div_parent = div2.div_code");
                sql.AppendLine("left join   db_status as status");
                sql.AppendLine("on          status.status_value = div1.div_type");
                sql.AppendLine("            and status.table_name = 'su_division'");
                sql.AppendLine("            and status.column_name = 'div_type'");
                sql.AppendLine("left join	db_location as locat");
                sql.AppendLine("on			locat.company_code = div1.company_code");
                sql.AppendLine("			and locat.location_code = div1.location_code");
                //sql.AppendLine("			and div1.div_type = 'L'");
                sql.AppendLine("left join	db_fac as fac");
                sql.AppendLine("on			fac.company_code = div1.company_code");
                sql.AppendLine("			and fac.fac_code = div1.fac_code");
                //sql.AppendLine("			and div1.div_type = 'F'");
                sql.AppendLine("left join	db_program as program");
                sql.AppendLine("on			program.company_code = div1.company_code");
                sql.AppendLine("			and program.program_code = div1.program_code");
                //sql.AppendLine("			and div1.div_type = 'P'");
                sql.AppendLine("where       div1.company_code = @CompanyCode");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("        and concat(div1.div_code, div1.div_name_tha, div1.div_name_eng) ilike concat('%', @Keyword, '%')");

                var divisions = await _context.GetPage(sql.ToString(), new { Language = this._user.Language, CompanyCode = request.CompanyCode, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);

                return new Division { Company = company, Divisions = divisions };
            }
        }
    }
}
