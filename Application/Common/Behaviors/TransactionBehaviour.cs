using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviors
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICleanDbContext _context;
        public TransactionBehaviour(ICleanDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);

            try
            {
                if (_context.HasActiveTransaction || !(request is ICommand || request is ICommand<TResponse>))
                {
                    return await next();
                }

                using (var transaction = await _context.BeginTransactionAsync())
                {
                    response = await next();

                    await _context.CommitTransactionAsync(transaction);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
