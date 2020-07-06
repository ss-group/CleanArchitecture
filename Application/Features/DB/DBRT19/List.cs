using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT19
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

            public Handler(ICleanDbContext context)
            {
                _context = context;
            }
            public async Task<PageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT degree_id as \"degreeId\" ");
                sql.AppendLine(" ,degree_name_tha as \"degreeNameTha\" ");
                sql.AppendLine(" ,degree_name_eng as \"degreeNameEng\" ");
                sql.AppendLine(" ,degree_short_name_tha as\"degreeShortNameTha\" ");
                sql.AppendLine(" ,degree_short_name_eng as\"degreeShortNameEng\" ");
                sql.AppendLine(" ,active as\"active\" ");
                sql.AppendLine(" ,xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_degree");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(degree_name_tha,degree_name_eng,degree_short_name_tha,degree_short_name_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
