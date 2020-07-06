using Application.Common.Interfaces;
using Domain.Types;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> SystemCodes { get; set; }
            public IEnumerable<dynamic> LangCodes { get; set; }
            public IEnumerable<dynamic> MainMenus { get; set; }
            public IEnumerable<dynamic> ProgramCodes { get; set; }
        }

        public class Query : IRequest<MasterData>
        {
            public string FieldName { get; set; }
            public string SystemCode { get; set; }
            public string MenuCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, MasterData>
        {
            private readonly ICleanDbContext _context;

            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<MasterData> Handle(Query request, CancellationToken cancellationToken)
            {
                var master = new MasterData();
                StringBuilder sql = new StringBuilder();

                switch (request.FieldName)
                {
                    // SystemCodeAndLanguage
                    case "SystemCodeAndLanguage":
                        sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END AS text,");
                        sql.AppendLine("            status_value AS value");
                        sql.AppendLine("FROM        db_status");
                        sql.AppendLine("WHERE       table_name = @TableName");
                        sql.AppendLine("            AND column_name = @ColumnName");
                        sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                        sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                        var systemCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language,  Tha = "th", TableName = "su_menu", ColumnName = "system_code" }, cancellationToken);

                        sql = new StringBuilder();
                        sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END AS text,");
                        sql.AppendLine("            status_value::Lang AS value");
                        sql.AppendLine("FROM        db_status");
                        sql.AppendLine("WHERE       table_name = @TableName");
                        sql.AppendLine("            AND column_name = @ColumnName");
                        sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                        sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                        var langCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName = "lang", ColumnName = "lang_code" }, cancellationToken);

                        master = new MasterData { SystemCodes = systemCodes, LangCodes = langCodes };
                        break;

                    // MainMenusAndProgramCodes
                    case "MainMenusAndProgramCodes":
                        sql.AppendLine("SELECT		CONCAT(a.menu_code, ' : ', CASE @Language WHEN @Tha THEN b.menuNameTha ELSE COALESCE(b.menuNameEng, b.menuNameTha) END) AS text,");
                        sql.AppendLine("            a.menu_code AS value,");
                        sql.AppendLine("            a.active");
                        sql.AppendLine("FROM 		su_menu AS a");
                        sql.AppendLine("INNER JOIN 	(SELECT	    a.menu_code,");
                        sql.AppendLine("                        a.menu_name AS menuNameTha,");
                        sql.AppendLine("                        b.menu_name AS menuNameEng");
                        sql.AppendLine("            FROM 	    su_menu_label AS a");
                        sql.AppendLine("            LEFT JOIN   (SELECT menu_code, menu_name FROM su_menu_label WHERE lang_code = @Eng::Lang) AS b");
                        sql.AppendLine("            ON			a.menu_code = b.menu_code");
                        sql.AppendLine("            WHERE 		a.lang_code = @Tha::Lang) AS b");
                        sql.AppendLine("ON	        a.menu_code = b.menu_code");
                        sql.AppendLine("WHERE       a.system_code = @SystemCode");

                        if (!string.IsNullOrEmpty(request.MenuCode))
                            sql.AppendLine("        AND a.menu_code != @MenuCode");

                        sql.AppendLine("ORDER BY    a.menu_code");
                        var mainMenus = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", Eng = "en", SystemCode = request.SystemCode, MenuCode = request.MenuCode }, cancellationToken);

                        sql = new StringBuilder();
                        sql.AppendLine("SELECT		CONCAT(program_code, ' : ', program_name) AS text,");
                        sql.AppendLine("            program_code AS value");
                        sql.AppendLine("FROM        su_program");
                        sql.AppendLine("WHERE       system_code = @SystemCode");
                        sql.AppendLine("ORDER BY    program_code");
                        var programCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { SystemCode = request.SystemCode }, cancellationToken);

                        master = new MasterData { MainMenus = mainMenus, ProgramCodes = programCodes };
                        break;
                }


                return master;
            }
        }
    }
}
