using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
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

                sql.AppendLine("    SELECT      CASE @Language WHEN @Tha THEN b.status_desc_tha ELSE COALESCE(b.status_desc_eng, b.status_desc_tha) END AS \"system\",");
                sql.AppendLine("                a.module_code as \"moduleCode\",CASE @Language WHEN @Tha THEN c.status_desc_tha ELSE COALESCE(c.status_desc_eng, c.status_desc_tha) END AS \"module\",");
                sql.AppendLine("                a.program_code AS \"programCode\",");
                sql.AppendLine("                a.program_name AS \"programName\",");
                sql.AppendLine("                a.program_path AS \"programPath\",");
                sql.AppendLine("                a.xmin AS \"rowVersion\"");
                sql.AppendLine("    FROM        su_program AS a");
                sql.AppendLine("    LEFT JOIN  db_status AS b");
                sql.AppendLine("    ON          a.system_code = b.status_value");
                sql.AppendLine("                AND b.table_name = @TableName_B");
                sql.AppendLine("                AND b.column_name  = @ColumnName_B");
                sql.AppendLine("    LEFT JOIN  db_status AS c");
                sql.AppendLine("    ON          a.module_code = c.status_value");
                sql.AppendLine("                AND c.table_name = @TableName_C");
                sql.AppendLine("                AND c.column_name  = @ColumnName_C");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("WHERE       CONCAT(b.status_desc_tha,");
                    sql.AppendLine("                   b.status_desc_eng,");
                    sql.AppendLine("                   c.status_desc_tha,");
                    sql.AppendLine("                   c.status_desc_eng,");
                    sql.AppendLine("                   a.program_code,");
                    sql.AppendLine("                   a.program_name)");
                    sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                }

                return await _context.GetPage(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName_B = "su_menu", ColumnName_B = "system_code", TableName_C = "su_program", ColumnName_C = "module_code", Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
