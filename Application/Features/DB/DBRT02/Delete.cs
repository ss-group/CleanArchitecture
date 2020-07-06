using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT02
{
    public class Delete
    {
        public class Command : ICommand
        {
            public int RegionId { get; set; }
            public uint RowVersion { get; set; }

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var region = new DbRegion { RegionId = request.RegionId, RowVersion = request.RowVersion };

                _context.Set<DbRegion>().Attach(region);
                _context.Set<DbRegion>().Remove(region);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
