using Application.Features;
using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Entities.SU;
using Domain.Types;
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
        private IDbContextTransaction _currentTransaction;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public bool HasActiveTransaction => _currentTransaction != null;
        public CleanDbContext(DbContextOptions<CleanDbContext> options, ICurrentUserAccessor currentUserAccessor)
         : base(options)
        {
            _currentUserAccessor = currentUserAccessor;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Lang>();
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
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = this._currentUserAccessor.UserName;
                        entry.Entity.CreatedProgram = this._currentUserAccessor.ProgramCode;
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = this._currentUserAccessor.UserName;
                        entry.Entity.UpdatedProgram = this._currentUserAccessor.ProgramCode;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        entry.Entity.UpdatedBy = this._currentUserAccessor.UserName;
                        entry.Entity.UpdatedProgram = this._currentUserAccessor.ProgramCode;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EntityTypes().Configure(e => e.Relational().TableName = e.ClrType.Name.ToLowercaseNamingConvention());
            modelBuilder.Properties().Where(p => p.Name != "RowVersion").Configure(p => p.Relational().ColumnName = p.Name.ToLowercaseNamingConvention());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanDbContext).Assembly);
        }
    }
}
