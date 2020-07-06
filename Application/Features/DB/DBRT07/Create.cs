using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT07
{
    public class Create
    {
        public class Command : DbPreName, ICommand<long>
        {

        }
        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbPreName>().Add((DbPreName)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.PreNameId;
            }
        }
    }
}
