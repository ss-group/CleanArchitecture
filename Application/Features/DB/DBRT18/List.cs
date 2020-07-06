using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT18
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
                sql.AppendLine(" SELECT distinct(g.list_item_group_code) as \"listItemGroupCode\" ");
                sql.AppendLine(" ,g.list_item_group_name as \"listItemGroupName\" ");
                sql.AppendLine(" ,g.system_code as \"systemCode\" ");
                sql.AppendLine(" ,g.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_list_item i");
                sql.AppendLine("right join db_list_item_group g on i.list_item_group_code = g.list_item_group_code");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(g.list_item_group_code,g.list_item_group_name,g.system_code) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
