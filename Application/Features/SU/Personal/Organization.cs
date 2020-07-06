using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Personal
{
    public class Organization
    {

        public class OrganizationVm
        {
            public IEnumerable<dynamic> Companies { get; set; }
            public IEnumerable<dynamic> Divisions { get; set; }
        }

        public class Query : IRequest<OrganizationVm>
        {
            public string Name { get; set; }

            public string Lang { get; set; }
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, OrganizationVm>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<OrganizationVm> Handle(Query request, CancellationToken cancellationToken)
            {
                var organize = new OrganizationVm();

                if (request.Name == "company")
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("	select c.company_code  as value,case @Lang when 'th' then company_name_tha else coalesce(company_name_eng ,company_name_tha) end as text	");
                    sql.AppendLine("          ,p.is_default as default ");
                    sql.AppendLine("	from su_company c 	");
                    sql.AppendLine("	inner join su_user_permission p on p.company_code = c.company_code 	");
                    sql.AppendLine("	where p.user_id  = @UserId ");
                    organize.Companies = await _context.QueryAsync<dynamic>(sql.ToString(), new { UserId = _user.UserId, Lang = request.Lang }, cancellationToken);
                }
                else if(request.Name == "division")
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("	select d.div_code as value	");
                    sql.AppendLine("	       ,concat(d.div_code,' : ',case @Lang when 'th' then div_name_tha else coalesce(div_name_eng ,div_name_tha) end) as text	");
                    sql.AppendLine("	from su_division d 	");
                    sql.AppendLine("	inner join su_user_division  u on u.company_code  = d.company_code 	");
                    sql.AppendLine("	                              and u.div_code  = d.div_code 	");
                    sql.AppendLine("	where u.user_id  =  @UserId ");
                    sql.AppendLine("	and u.company_code  = @CompanyCode	");
                    sql.AppendLine("	order by value	");
                    organize.Divisions = await _context.QueryAsync<dynamic>(sql.ToString(), new { CompanyCode = request.CompanyCode, UserId = _user.UserId, Lang = request.Lang }, cancellationToken);
                }
                return organize;
            }
        }
    }
}
