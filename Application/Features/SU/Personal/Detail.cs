using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Personal
{
    public class Detail
    {

        public class PersonalVm
        {
            public string PersonalCode { get; set; }
            public string PersonalName { get; set; }
        }

        public class Query : IRequest<PersonalVm>
        {
        }

        public class Handler : IRequestHandler<Query, PersonalVm>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<PersonalVm> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("	select e.employee_code as personalCode, get_name(@Lang,e.t_name_concat,e.e_name_concat) as personalName ");
                sql.AppendLine("	from su_user_type t 	");
                sql.AppendLine("	inner join db_employee e on e.company_code = t.company_code 	");
                sql.AppendLine("	                        and e.employee_code = t.employee_code 	");
                sql.AppendLine("    where t.user_id = @UserId ");
                sql.AppendLine("    limit 1 ");

                return await _context.QueryFirstOrDefaultAsync<PersonalVm>(sql.ToString(), new { UserId = _user.UserId,Lang = _user.Language }, cancellationToken);
            }
        }
    }
}
