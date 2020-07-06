using Application.Features;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersistenseOracle
{
    public partial class OracleDbContext : DbContext, IOracleDbContext 
    {
        private IDbContextTransaction _currentTransaction;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public bool HasActiveTransaction => _currentTransaction != null;
        public OracleDbContext(DbContextOptions<OracleDbContext> options, ICurrentUserAccessor currentUserAccessor)
         : base(options)
        {
            _currentUserAccessor = currentUserAccessor;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.SetAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            this.SetAudit();
            return base.SaveChanges();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false);

            return _currentTransaction;
        }
        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await base.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        private void SetAudit()
        {
            foreach (var entry in ChangeTracker.Entries<EntityBaseOracle>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CrDate = DateTime.Now;
                        entry.Entity.CrBy = this._currentUserAccessor.UserName;
                        entry.Entity.ProgId = this._currentUserAccessor.ProgramCode;
                        entry.Entity.UpdDate = DateTime.Now;
                        entry.Entity.UpdBy = this._currentUserAccessor.UserName;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdDate = DateTime.Now;
                        entry.Entity.UpdBy = this._currentUserAccessor.UserName;
                        entry.Entity.ProgId = this._currentUserAccessor.ProgramCode;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EntityTypes().Configure(e => e.Relational().TableName = e.ClrType.Name.ToUpperNamingConvention());
            modelBuilder.Properties().Configure(p => p.Relational().ColumnName = p.Name.ToUpperNamingConvention());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OracleDbContext).Assembly);
        }
    }
}
