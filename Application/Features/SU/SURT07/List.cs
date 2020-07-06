using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT07
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
        }

        public class Handler : IRequestHandler<Query, PageDto>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("        select 	password_policy_code as \"passwordPolicyCode\",");
                sql.AppendLine("		        password_policy_name as \"passwordPolicyName\",");
                sql.AppendLine("		        password_policy_description as \"passwordPolicyDescription\",");
                sql.AppendLine("		        active,");
                sql.AppendLine("                xmin as \"rowVersion\"");
                sql.AppendLine("        from 	su_password_policy");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("    where   concat(password_policy_code,");
                    sql.AppendLine("                   password_policy_name,");
                    sql.AppendLine("                   password_policy_description)");
                    sql.AppendLine("            ilike concat('%', @Keyword, '%')");
                }

                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
