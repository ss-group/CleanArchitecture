using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
{
    public class List
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
            public string CompanyCode { get; set; }
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
                request.CompanyCode = _user.Company;
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT et.education_type_code as \"educationTypeCode\",et.education_type_code_mua as \"educationTypeCodeMua\" ");
                sql.AppendLine(",et.education_type_name_tha as \"educationTypeNameTha\",et.education_type_name_eng as \"educationTypeNameEng\" ");
                sql.AppendLine(",et.education_short_name_tha as \"educationShortNameTha\",et.education_short_name_eng as \"educationShortNameEng\" ");
                sql.AppendLine(",et.summary_year as \"summaryYear\",et.new_level_code as \"newLevelCode\" ");
                sql.AppendLine(",get_name(@Lang,etl.education_type_level_name_tha,etl.education_type_level_name_eng) as \"typeLevel\" ");
                sql.AppendLine(",et.format_code  as \"formatCode\",et.active,et.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_education_type et ");
                sql.AppendLine("JOIN db_education_type_level etl ON (etl.education_type_level = et.type_level AND etl.company_code = @Company ) ");
                sql.AppendLine("WHERE et.company_code = @Company ");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("AND CONCAT(get_name(@Lang,etl.education_type_level_name_tha,etl.education_type_level_name_eng) ");
                    sql.AppendLine(",et.education_type_code_mua,et.education_type_code ");
                    sql.AppendLine(",et.education_type_name_tha,et.education_type_name_eng ");
                    sql.AppendLine(",et.education_short_name_tha,et.education_short_name_eng ) ILIKE '%' || @Keyword || '%' ");
                }
                return await _context.GetPage(sql.ToString(), new { Company = _user.Company, Keyword = request.Keyword, Lang = _user.Language }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
