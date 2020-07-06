using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT17
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
                sql.AppendLine(" SELECT building_id as \"buildingId\" ");
                sql.AppendLine(" ,building_code as \"buildingCode\" ");
                sql.AppendLine(" ,building_name_tha as \"buildingNameTha\" ");
                sql.AppendLine(" ,building_name_eng as \"buildingNameEng\" ");
                sql.AppendLine(" ,active as \"active\" ");
                sql.AppendLine(" ,xmin as \"rowVersion\" ");
                sql.AppendLine(" FROM db_building ");
                sql.AppendLine(" WHERE db_building.company_code = @Company ");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(building_code,building_name_tha,building_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword, Company = _user.Company }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
