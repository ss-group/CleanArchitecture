using Application.Features;
using Domain.Entities.SU;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// ef core for manage data , dapper for query & store procedure same connection & transaction
    /// </summary>
    public interface ICleanDbContext
    {
        // ef core
        DatabaseFacade Database { get; }
        bool HasActiveTransaction { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        void RollbackTransaction();
        DbSet<T> Set<T>() where T : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        //dapper
        Task<PageDto> GetPage(string sql, object param, RequestPageQuery page, CancellationToken token=default);
        Task<int> ExecuteAsync(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<T> ExecuteScalarAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<T> QueryFirstAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<T> QuerySingleAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param, CancellationToken token = default, bool isStore = false);

        //extension
        Task<T> GetParameterValue<T>(string group, string code, CancellationToken token = default);
    }
}
