using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT08
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string EmployeeCode { get; set; }
            public string EmployeeName { get; set; }
            public string DivCode { get; set; }
            public string GroupTypeCode { get; set; }
            public string TurnoverDate { get; set; }
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
                sql.AppendLine(" SELECT e.employee_code AS \"employeeCode\" ");
                sql.AppendLine(" ,get_name(@Lang, e.t_name_concat, e.e_name_concat) AS \"employeeName\" ");
                sql.AppendLine(" ,get_name(@Lang, d.div_name_tha, d.div_name_eng) AS \"divName\" ");
                sql.AppendLine(" ,get_name(@Lang,li.list_item_name_tha,li.list_item_name_eng) AS \"groupTypeCode\" ");
                sql.AppendLine(" ,get_name(@Lang,li.list_item_name_tha,li.list_item_name_eng) AS \"groupTypeCode\" ");
                sql.AppendLine(" ,e.turnover_date AS \"turnoverDate\" ");
                sql.AppendLine(" ,e.xmin as \"rowVersion\" ");
                sql.AppendLine(" From db_employee e");
                sql.AppendLine(" LEFT JOIN su_division d on e.div_code = d.div_code");
                sql.AppendLine(" AND d.company_code = @company ");
                sql.AppendLine(" LEFT JOIN db_list_item li on e.group_type_code = li.list_item_code");
                sql.AppendLine(" AND li.list_item_group_code = 'GroupTypeCode' ");
                sql.AppendLine(" WHERE e.company_code = @company ");
                if (!string.IsNullOrWhiteSpace(request.EmployeeCode))
                    sql.AppendLine(" AND e.employee_code ILIKE '%' || @employeeCode || '%'");
                if(!string.IsNullOrWhiteSpace(request.EmployeeName))
                    sql.AppendLine(" AND get_name(@Lang, e.t_name_concat, e.e_name_concat) ILIKE '%' || @employeeName || '%'");
                if(!string.IsNullOrWhiteSpace(request.DivCode))
                    sql.AppendLine(" AND e.div_code = @divCode "); 
                if (!string.IsNullOrWhiteSpace(request.GroupTypeCode))
                    sql.AppendLine(" AND e.group_type_code = @groupTypeCode ");
                if (request.TurnoverDate == "2")
                {
                    sql.AppendLine(" AND e.turnover_date IS NULL ");
                }

                if (request.TurnoverDate == "3")
                {
                    sql.AppendLine(" AND e.turnover_date is NOT NULL ");
                }

                return await _context.GetPage(sql.ToString(), new { company = _user.Company, 
                    employeeCode = request.EmployeeCode, 
                    employeeName = request.EmployeeName, 
                    divCode = request.DivCode, 
                    groupTypeCode = request.GroupTypeCode,
                    turnoverDate = request.TurnoverDate,
                    lang = _user.Language 
                }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
