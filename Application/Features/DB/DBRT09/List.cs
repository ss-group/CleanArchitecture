using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT09
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
                sql.AppendLine("SELECT education_type_level as \"educationTypeLevel\" ");
                sql.AppendLine(" , prefix as \"prefix\", format_code as \"formatCode\", education_type_code_mua as \"educationTypeCodeMua\" ");
                sql.AppendLine(" , education_type_level_name_tha as \"educationTypeLevelNameTha\",education_type_level_name_eng as \"educationTypeLevelNameEng\" ");
                sql.AppendLine(" , education_level_short_name_tha as \"educationLevelShortNameTha\",education_level_short_name_eng as \"educationLevelShortNameEng\" ");
                sql.AppendLine(" , graduated_name_tha as \"graduatedNameTha\", graduated_name_eng as \"graduatedNameEng\" ");
                sql.AppendLine(" , list_item_name_tha as \"groupLevel\",db_education_type_level.active ");
                sql.AppendLine(" , db_education_type_level.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_education_type_level ");
                sql.AppendLine("JOIN db_list_item ON (db_education_type_level.group_level = db_list_item.list_item_code AND list_item_group_code = 'EduGroupLevel' ) ");
                sql.AppendLine("WHERE db_education_type_level.company_code = @Company");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(education_type_level,prefix,format_code,education_type_code_mua,education_type_level_name_tha,education_type_level_name_eng,education_level_short_name_tha,education_level_short_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Company = _user.Company, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
