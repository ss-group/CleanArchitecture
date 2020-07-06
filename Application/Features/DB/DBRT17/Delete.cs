using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT17
{
    public class Delete
    {
        public class Command : ICommand
        {

            public long BuildingId { get; set; }
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
                var building = new DbBuilding { BuildingId = request.BuildingId, RowVersion = request.RowVersion };
                _context.Set<DbBuilding>().Attach(building);
                _context.Set<DbBuilding>().Remove(building);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
