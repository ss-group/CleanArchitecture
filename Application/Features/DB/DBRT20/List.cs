using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT20
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
                sql.AppendLine(" SELECT department_code as \"departmentCode\" ");
                sql.AppendLine(" ,department_code_mua as \"departmentCodeMua\" ");
                sql.AppendLine(" ,department_name_tha as \"departmentNameTha\" ");
                sql.AppendLine(" ,department_name_eng as\"departmentNameEng\" ");
                sql.AppendLine(" ,active as\"active\" ");
                sql.AppendLine(" ,xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_department");
                sql.AppendLine("WHERE company_code = @Company");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(department_code, department_code_mua, department_name_tha, department_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Company = _user.Company }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
