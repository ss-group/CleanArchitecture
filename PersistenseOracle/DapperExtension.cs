using Application.Features;
using Application.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PersistenseOracle
{
    public partial class OracleDbContext : DbContext, IOracleDbContext
    { 
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
