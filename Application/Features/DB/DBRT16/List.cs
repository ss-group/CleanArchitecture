using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT16
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
                sql.AppendLine(" SELECT company_code as \"companyCode\" ");
                sql.AppendLine(" ,location_code as \"locationCode\" ");
                sql.AppendLine(" ,location_name_tha as \"locationNameTha\" ");
                sql.AppendLine(" ,location_name_eng as \"locationNameEng\" ");
                sql.AppendLine(" ,active as \"active\" ");
                sql.AppendLine(" ,xmin as \"rowVersion\" ");
                sql.AppendLine(" FROM db_location ");
                sql.AppendLine(" WHERE company_code = @Company ");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(location_code,location_name_tha,location_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Company = _user.Company, Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
