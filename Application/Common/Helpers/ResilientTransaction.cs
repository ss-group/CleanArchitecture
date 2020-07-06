using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public class ResilientTransaction
    {
        private ICleanDbContext _context;

        private CancellationToken cancellationToken => default;
        private ResilientTransaction(ICleanDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public static ResilientTransaction New(ICleanDbContext context) =>
            new ResilientTransaction(context);

        public async Task ExecuteAsync(Func<Task> action)
        {
            if (_context != null)
            {
                using (var transaction = await _context.BeginTransactionAsync())
                {
                    await action();
                    await _context.CommitTransactionAsync(transaction);
                }
            }
        }
    }
}
