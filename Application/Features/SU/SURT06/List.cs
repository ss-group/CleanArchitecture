using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Username { get; set; }
            public string Employee { get; set; }
            public string Status { get; set; }
            public string UserType { get; set; }

            public string UserGroup { get; set; }
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
                var student = await _context.GetParameterValue<string>("SuUserType", "Student", cancellationToken);

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("	select u.user_id as id,u.user_name as \"username\" ");
                if(request.UserType == student)
                {
                    sql.AppendLine("          ,e.student_code as \"employeeCode\" ");
                    sql.AppendLine("	      , get_name(@Lang,e.student_name_tha,e.student_name_eng) as \"employeeName\" ");
                }
                else
                {
                    sql.AppendLine("          ,e.employee_code as \"employeeCode\" ");
                    sql.AppendLine("	      , get_name(@Lang,e.t_name_concat,e.e_name_concat) as \"employeeName\" ");
                }

                sql.AppendLine("	      , u.active,u.force_change_password as \"forceChange\",case  when u.lockout_end >= current_timestamp then true else false end as locked	");
                sql.AppendLine("	      , u.xmin as \"rowVersion\"	");
                sql.AppendLine("	from su_user u 	");
                sql.AppendLine("	inner join su_user_type t on t.user_id  = u.user_id 	");
                if(request.UserType == student)
                {
                    sql.AppendLine("	inner join sh_student e on e.company_code = t.company_code 	");
                    sql.AppendLine("	                       and e.student_id = t.student_id 	");
                }
                else
                {
                    sql.AppendLine("	inner join db_employee e on e.company_code = t.company_code 	");
                    sql.AppendLine("	                        and e.employee_code = t.employee_code 	");
                    sql.AppendLine("                            and group_type_code = @UserGroup ");
                }

                sql.AppendLine("    where 1=1 ");

                if (!string.IsNullOrEmpty(request.Username))
                {
                    sql.AppendLine("and u.user_name ilike concat('%',@Username,'%') ");
                }

                if (request.UserType == student && !string.IsNullOrEmpty(request.Employee))
                {
                    sql.AppendLine("and replace(concat(e.student_code,e.student_name_tha,e.student_name_eng),' ','') ilike  concat('%',replace(@Employee,' ',''),'%') ");
                }
                else if(request.UserType != student && !string.IsNullOrEmpty(request.Employee))
                {
                    sql.AppendLine("and replace(concat(e.employee_code,e.t_name_concat,e.e_name_concat),' ','') ilike  concat('%',replace(@Employee,' ',''),'%') ");
                }

                switch (request.Status)
                {
                    case "A":
                        sql.AppendLine("  AND u.active = true ");
                        break;
                    case "I":
                        sql.AppendLine("  AND coalesce(u.active,false) = false");
                        break;
                    case "F":
                        sql.AppendLine("  AND u.force_change_password = true ");
                        break;
                    case "L":
                        sql.AppendLine("  AND u.lockout_end is not null ");
                        sql.AppendLine("  AND current_timestamp <= lockout_end  ");
                        break;
                }

                return await _context.GetPage(sql.ToString(), new
                {
                    Company = this._user.Company,
                    Username = request.Username,
                    Employee = request.Employee,
                    Status = request.Status,
                    UserGroup = request.UserGroup,
                    Lang = _user.Language
                }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
