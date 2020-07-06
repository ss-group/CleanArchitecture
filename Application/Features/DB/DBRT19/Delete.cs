using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT19
{
    public class Delete
    {
        public class Command : ICommand
        {

            public long DegreeId { get; set; }

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
                var degreetype = new DbDegree { DegreeId = request.DegreeId, RowVersion = request.RowVersion };
                _context.Set<DbDegree>().Attach(degreetype);
                _context.Set<DbDegree>().Remove(degreetype);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
