    using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT07
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
                sql.AppendLine("SELECT      a.pre_name_id as \"preNameId\",");
                sql.AppendLine("            a.pre_name_code_mua as \"preNameCodeMua\",");
                sql.AppendLine("            a.pre_name_tha as \"preNameTha\",");
                sql.AppendLine("            a.pre_name_eng as \"preNameEng\",");
                sql.AppendLine("            a.pre_name_short_tha as\"preNameShortTha\",");
                sql.AppendLine("            a.pre_name_short_eng as\"preNameShortEng\",");
                sql.AppendLine("            CASE @Language WHEN @Tha THEN list_item_name_tha ELSE COALESCE(list_item_name_tha) END as\"preNameType\",");
                sql.AppendLine("            a.active,a.xmin as \"rowVersion\"");
                sql.AppendLine("FROM        db_pre_name as a");
                sql.AppendLine("RIGHT JOIN  db_list_item as b");
                sql.AppendLine("ON          a.pre_name_type = b.list_item_code");
                sql.AppendLine("WHERE       b.list_item_group_code =  'PreNameType'");
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    sql.AppendLine("AND CONCAT(pre_name_id,' ',pre_name_code_mua,' ',pre_name_tha,' ',pre_name_eng,' ',pre_name_short_tha,' ',pre_name_short_eng) ILIKE '%' || @Keyword || '%'");
                return await _context.GetPage(sql.ToString(), new { Language = this._user.Language, Company = this._user.Company, Tha = "th", TableName = "db_pre_name", ColumnName = "pre_name_type", Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
