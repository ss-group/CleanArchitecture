using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT18
{
    public class Detail
    {
        public class Query : RequestPageQuery, IRequest<PageDto>
        {
            public string Keyword { get; set; }
            public string ListItemGroupCode { get; set; }
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
                sql.AppendLine(" SELECT g.list_item_group_code as \"listItemGroupCode\" ");
                sql.AppendLine(" ,i.list_item_code as \"listItemCode\" ");
                sql.AppendLine(" ,i.list_item_code_mua as \"listItemCodeMua\" ");
                sql.AppendLine(" ,i.list_item_name_tha as \"listItemNameTha\" ");
                sql.AppendLine(" ,i.list_item_name_eng as \"listItemNameEng\" ");
                sql.AppendLine(" ,i.list_item_short_name_tha as \"listItemShortNameTha\" ");
                sql.AppendLine(" ,i.list_item_short_name_eng as \"listItemShortNameEng\" ");
                sql.AppendLine(" ,i.active as \"active\" ");
                sql.AppendLine(" ,i.xmin as \"rowVersion\" ");
                sql.AppendLine("FROM db_list_item i");
                sql.AppendLine("inner join db_list_item_group g on i.list_item_group_code = g.list_item_group_code");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("WHERE CONCAT(list_item_code,list_item_code_mua,list_item_name_tha,list_item_name_eng,list_item_short_name_tha,list_item_short_name_eng) ILIKE '%' || @Keyword || '%'");
                if (!string.IsNullOrWhiteSpace(request.ListItemGroupCode))
                    sql.AppendLine("AND g.list_item_group_code::text = @ListItemGroupCode");
                    sql.AppendLine("ORDER BY i.sequence ASC");
                return await _context.GetPage(sql.ToString(), new { ListItemGroupCode = request.ListItemGroupCode, Keyword = request.Keyword}, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
