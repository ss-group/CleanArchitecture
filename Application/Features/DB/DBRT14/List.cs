using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT14
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
                sql.AppendLine(" SELECT p.project_id AS \"projectId\" ");
                sql.AppendLine(" ,p.project_code AS \"projectCode\" ");
                sql.AppendLine(" ,p.project_name_tha AS \"projectNameTha\" ");
                sql.AppendLine(" ,p.project_name_eng AS \"projectNameEng\" ");
                sql.AppendLine(" ,get_name(@Lang, et.education_type_name_tha, et.education_type_name_eng) AS \"educationTypeNameTha\" ");
                sql.AppendLine(" ,get_name(@Lang, f.fac_name_tha, f.fac_name_eng) AS \"facNameTha\" ");
                sql.AppendLine(" ,get_name(@Lang, st.student_type_name, st.student_type_name_eng) AS \"studentTypeName\" ");
                sql.AppendLine(" ,p.erp_code AS \"erpCode\" ");
                sql.AppendLine(" ,p.erp_activity AS \"erpActivity\" ");
                sql.AppendLine(" ,p.erp_product AS \"erpProduct\" ");
                sql.AppendLine(" ,p.erp_project AS \"erpProject\" ");
                sql.AppendLine(" ,p.active AS \"active\" ");
                sql.AppendLine(" ,p.xmin AS \"rowVersion\" ");
                sql.AppendLine(" FROM db_project p");
                sql.AppendLine(" LEFT JOIN db_education_type et ON p.education_type_code = et.education_type_code ");
                sql.AppendLine(" AND et.company_code = @Company  ");
                sql.AppendLine(" LEFT JOIN db_fac f on p.fac_code = f.fac_code ");
                sql.AppendLine(" AND f.company_code = @Company  ");
                sql.AppendLine(" LEFT JOIN sh_student_type st on p.student_type_code = st.student_type_code ");
                sql.AppendLine(" WHERE p.company_code = @Company  ");
                if (!string.IsNullOrWhiteSpace(request.Keyword)){
                    sql.AppendLine("AND CONCAT(project_code, project_name_tha, project_name_eng, get_name(@Lang, et.education_type_name_tha, et.education_type_name_eng), get_name(@Lang, f.fac_name_tha, f.fac_name_eng), get_name(@Lang, st.student_type_name, st.student_type_name_eng), erp_code, erp_activity, erp_product, erp_project ) ILIKE '%' || @Keyword || '%'");
                }
                return await _context.GetPage(sql.ToString(),new { lang = _user.Language, Company = _user.Company, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
