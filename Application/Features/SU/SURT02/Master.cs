using Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Master
    {
        public class MasterData
        {
            public IEnumerable<dynamic> SystemCodes { get; set; }
            public IEnumerable<dynamic> ModuleCodes { get; set; }
            public IEnumerable<dynamic> Languages { get; set; }
        }

        public class Query : IRequest<MasterData>
        {

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
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END AS text,");
                sql.AppendLine("            status_value AS value");
                sql.AppendLine("FROM        db_status");
                sql.AppendLine("WHERE       table_name = @TableName");
                sql.AppendLine("            AND column_name = @ColumnName");
                sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                var systemCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName = "su_menu", ColumnName = "system_code" }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine("SELECT      CONCAT(status_value, ' : ', CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END) AS text,");
                sql.AppendLine("            status_value AS value");
                sql.AppendLine("FROM        db_status");
                sql.AppendLine("WHERE       table_name = @TableName");
                sql.AppendLine("            AND column_name = @ColumnName");
                sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                var moduleCodes = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName = "su_program", ColumnName = "module_code" }, cancellationToken);

                sql = new StringBuilder();
                sql.AppendLine("SELECT      CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END AS language");
                sql.AppendLine("FROM        db_status");
                sql.AppendLine("WHERE       table_name = @TableName");
                sql.AppendLine("            AND column_name = @ColumnName");
                sql.AppendLine("ORDER BY    COALESCE(sequence, 9999999),");
                sql.AppendLine("            CASE @Language WHEN @Tha THEN status_desc_tha ELSE COALESCE(status_desc_eng, status_desc_tha) END");
                var languages = await _context.QueryAsync<dynamic>(sql.ToString(), new { Language = this._user.Language, Tha = "th", TableName = "lang", ColumnName = "lang_code" }, cancellationToken);

                return new MasterData { SystemCodes = systemCodes, ModuleCodes = moduleCodes, Languages = languages };
            }
        }
    }
}
