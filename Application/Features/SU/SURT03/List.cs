using Application.Common.Interfaces;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
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

                sql.AppendLine("    SELECT		CASE @Language WHEN @Tha THEN d.status_desc_tha ELSE COALESCE(d.status_desc_eng, d.status_desc_tha) END AS \"systemCode\",");
                sql.AppendLine("                a.menu_code AS \"menuCode\",");
                sql.AppendLine("                a.main_menu AS \"mainMenu\",");
                sql.AppendLine("                a.program_code AS \"programCode\",");
                sql.AppendLine("                b.menuNameTha AS \"menuNameTha\",");
                sql.AppendLine("                b.menuNameEng AS \"menuNameEng\",");
                sql.AppendLine("                a.active,");
                sql.AppendLine("                a.xmin AS \"rowVersion\"");
                sql.AppendLine("    FROM		su_menu AS a");
                sql.AppendLine("    LEFT JOIN 	(SELECT		x.menu_code,");
                sql.AppendLine("                            x.menu_name AS menuNameTha,");
                sql.AppendLine("                            y.menu_name AS menuNameEng");
                sql.AppendLine("                FROM 	    su_menu_label AS x");
                sql.AppendLine("                LEFT JOIN   (SELECT menu_code, menu_name FROM su_menu_label WHERE lang_code = @Eng::Lang) AS y");
                sql.AppendLine("                ON			x.menu_code = y.menu_code");
                sql.AppendLine("                WHERE 		x.lang_code = @Tha::Lang) AS b");
                sql.AppendLine("    ON	 		a.menu_code = b.menu_code");
                sql.AppendLine("    LEFT JOIN 	db_status AS d");
                sql.AppendLine("    ON			a.system_code = d.status_value");
                sql.AppendLine("                AND d.table_name = @TableName");
                sql.AppendLine("                AND d.column_name = @ColumnName");

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    sql.AppendLine("WHERE       CONCAT(d.status_desc_tha,");
                    sql.AppendLine("                   d.status_desc_eng,");
                    sql.AppendLine("                   a.menu_code,");
                    sql.AppendLine("                   a.main_menu,");
                    sql.AppendLine("                   a.program_code,");
                    sql.AppendLine("                   b.menuNameTha,");
                    sql.AppendLine("                   b.menuNameEng)");
                    sql.AppendLine("            ILIKE CONCAT('%', @Keyword, '%')");
                }

                return await _context.GetPage(sql.ToString(), new { Language = this._user.Language, Tha = "th", Eng = "en", TableName = "su_menu", ColumnName = "system_code", Keyword = request.Keyword }, (RequestPageQuery)request, cancellationToken);
            }
        }
    }
}
