using Application.Common.Extensions;
using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class MasterAutocomplete
    {
        public class MasterData
        {
            public IEnumerable<dynamic> Employees { get; set; }
        }

        public class Query : IRequest<MasterData>
        {
            public string PersonCompany {get;set;}
            public long? StudentId { get; set; }

            public string EmployeeCode { get; set; }
            public bool IsStudent { get; set; }
            public string UserGroup { get; set; }

            public string Keyword { get; set; }

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
                var master = new MasterData();
                master.Employees = request.IsStudent ? await this.GetStudents( request, cancellationToken) : await this.GetEmployees( request,cancellationToken);
                return master;
            }

            private Task<IEnumerable<dynamic>> GetStudents(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("	select e.student_id as value,e.student_code as \"studentCode\",e.company_code as \"companyCode\" ");
                sql.AppendLine("          ,coalesce(e.id_card,e.passport_no) as \"personalId\",e.email");
                sql.AppendLine("	      ,concat(e.student_code,' : ',get_name(@Lang,e.student_name_tha,e.student_name_eng)) as text	");
                sql.AppendLine("	from sh_student e	");
                sql.AppendLine("    inner join sh_student_status ss on ss.status_code = e.status_code ");
                sql.AppendLine("	where 1=1 ");

                if (request.StudentId.HasValue)
                {
                    sql.AppendLine("    and e.student_id = @Id ");
                    sql.AppendLine("     and e.company_code = @PersonCompany ");
                }
                else
                {
                    sql.AppendLine("    and company_code = @Company ");
                    sql.AppendLine("    and exists(select 'x' from su_parameter p ");
                    sql.AppendLine("                          where p.parameter_group_code = 'StudentStatusGroup' ");
                    sql.AppendLine("                          and p.parameter_code = 'NormalStatus' ");
                    sql.AppendLine("                          and p.parameter_value = ss.status_group_code )  ");
                    sql.AppendLine("    and not exists(select 'x' ");
                    sql.AppendLine("                   from su_user_type t ");
                    sql.AppendLine("                   where t.company_code = e.company_code ");
                    sql.AppendLine("                   and t.student_id = e.student_id )");

                    if (!string.IsNullOrWhiteSpace(request.Keyword))
                    {
                        sql.AppendLine(" and concat(e.student_code,e.student_name_tha,e.student_name_eng) ilike concat('%',@Keyword,'%') ");
                    }
                    sql.AppendLine(" order by e.student_code desc ");
                    if (string.IsNullOrWhiteSpace(request.Keyword))
                        sql.AppendLine($"limit {20}");
                }
              
                return _context.QueryAsync<dynamic>(sql.ToString(), new { PersonCompany = request.PersonCompany,Company = _user.Company, Lang = _user.Language,Keyword = request.Keyword, Id = request.StudentId, }, cancellationToken);

            }
            private Task<IEnumerable<dynamic>> GetEmployees(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("	select e.employee_code as value,e.company_code as \"companyCode\",e.email ");
                sql.AppendLine("          ,personal_id as \"personalId\"");
                sql.AppendLine("	      ,concat(e.employee_code,' : ',get_name(@Lang,e.t_name_concat,e.e_name_concat)) as text	");
                sql.AppendLine("	from db_employee e	");
                sql.AppendLine("	where 1=1 ");

                if (!string.IsNullOrEmpty(request.EmployeeCode))
                {
                  sql.AppendLine(" and e.employee_code = @Employee ");
                  sql.AppendLine(" and e.company_code = @PersonCompany ");
                }
                else
                {
                    sql.AppendLine("    and company_code = @Company ");
                    sql.AppendLine("    and group_type_code = @UserGroup ");
                    sql.AppendLine("    and coalesce(turnover_date ::date,CURRENT_DATE + INTERVAL '1 day') > CURRENT_DATE ");
                    sql.AppendLine("    and not exists(select 'x' ");
                    sql.AppendLine("                   from su_user_type t ");
                    sql.AppendLine("                   where t.company_code = e.company_code ");
                    sql.AppendLine("                   and t.employee_code = e.employee_code )");

                    if (!string.IsNullOrWhiteSpace(request.Keyword))
                    {
                        sql.AppendLine(" and concat(e.employee_code,e.t_name_concat,e.e_name_concat) ilike concat('%',@Keyword,'%')");
                    }
                    sql.AppendLine(" order by e.employee_code desc ");
                    if (string.IsNullOrWhiteSpace(request.Keyword))
                        sql.AppendLine($"limit {20}");
                }
           
                return _context.QueryAsync<dynamic>(sql.ToString(), new { PersonCompany = request.PersonCompany, Company = _user.Company, Lang = _user.Language,UserGroup = request.UserGroup, Employee = request.EmployeeCode,Keyword = request.Keyword }, cancellationToken);
            }
        }
    }
}
