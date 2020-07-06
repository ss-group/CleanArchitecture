using Application.Common.Mapping;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.DB;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class Employee
    {
        public class EmployeeVm
        {
            public string EmployeeCode { get; set; }
            public long? StudentId { get; set; }
            public string PersonCompany {get;set;}
        }
        public class Query : IRequest<EmployeeVm>
        {
            public long UserId { get; set; }
            public string UserType { get; set; }
        }

        public class Handler : IRequestHandler<Query, EmployeeVm>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<EmployeeVm> Handle(Query request, CancellationToken cancellationToken)
            {
                var student = await _context.GetParameterValue<string>("SuUserType", "Student", cancellationToken);
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select e.company_code as person_company,");
                if (request.UserType == student)
                {
                    sql.AppendLine(" e.student_id ");
                }
                else
                {
                    sql.AppendLine(" e.employee_code ");
                }
                sql.AppendLine("from su_user_type t ");
                if(request.UserType == student)
                {
                    sql.AppendLine("inner join sh_student e on e.company_code = t.company_code ");
                    sql.AppendLine("and e.student_id  = t.student_id ");
                }
                else
                {
                    sql.AppendLine("inner join db_employee e on e.company_code = t.company_code ");
                    sql.AppendLine("and e.employee_code  = t.employee_code ");
                }

                sql.AppendLine("where t.user_id = @User");

                var emp = await _context.QueryFirstOrDefaultAsync<EmployeeVm>(sql.ToString(),new { User = request.UserId},cancellationToken);
 
                return emp;
            }
        }
    }
}
