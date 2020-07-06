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
    public class Delete
    {
        public class Command : ICommand
        {
            public long PreNameId { get; set; }
            public uint RowVersion { get; set; }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var institute = new DbPreName { PreNameId = request.PreNameId, RowVersion = request.RowVersion };

                _context.Set<DbPreName>().Attach(institute);
                _context.Set<DbPreName>().Remove(institute);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
