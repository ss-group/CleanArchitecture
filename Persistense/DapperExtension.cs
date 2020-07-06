using Application.Features;
using Application.Common.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Persistense
{
    public partial class CleanDbContext : DbContext, ICleanDbContext
    {
        public async Task<PageDto> GetPage(string sql, object param, RequestPageQuery page, CancellationToken token = default(CancellationToken))
        {
            string sort = string.Empty;
            if (page.Sort != null)
            {
                var sortPart = page.Sort.Split(",");
                string sortSQL = null;
                foreach (var column in sortPart)
                {
                    var columnPart = column.Split(" ");
                    sortSQL = string.Join(",", sortSQL, $"\"{columnPart[0]}\" {(columnPart.Length == 2 ? columnPart[1] : string.Empty)} ");
                }
                sort = $"ORDER BY {sortSQL.Substring(1, sortSQL.Length - 1)} ";
            }

            string query = $"WITH page AS ({sql}) SELECT page.*,TotalRow.Count AS {nameof(PageDto.Count)} FROM page CROSS JOIN (SELECT Count(1) AS Count FROM page) AS TotalRow {sort} OFFSET {page.Index * page.Size} ROWS FETCH NEXT {page.Size} ROWS ONLY";
            var result = await this.Database.GetDbConnection().QueryAsync<dynamic>(new CommandDefinition(query, param, cancellationToken: token));
            var firstRow = result.FirstOrDefault();
            return new PageDto
            {
                Rows = result,
                Count = firstRow != null ? (long)(firstRow as IDictionary<string, object>)["count"] : 0
            };

        }

        public Task<int> ExecuteAsync(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().ExecuteAsync(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().ExecuteScalarAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> QueryFirstAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().QueryFirstAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> QuerySingleAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().QuerySingleAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().QuerySingleOrDefaultAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CancellationToken token = default, bool isStore = false)
        {
            return this.Database.GetDbConnection().QueryAsync<T>(new CommandDefinition(sql, param, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: isStore ? CommandType.StoredProcedure : CommandType.Text));
        }

        public Task<T> GetParameterValue<T>(string group,string code,CancellationToken token)
        {
            return this.Database.GetDbConnection().ExecuteScalarAsync<T>(new CommandDefinition("SELECT parameter_value FROM su_parameter WHERE parameter_group_code = @Group AND parameter_code = COALESCE(@Code,parameter_code)",new {
                Group = group,
                Code = code
            }, this._currentTransaction?.GetDbTransaction(), cancellationToken: token, commandType: CommandType.Text));
        }
    }
}
