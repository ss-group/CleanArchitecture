using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
{
    public class Delete
    {
        public class Command : SuMenu, ICommand
        {

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
                var menu = await _context.Set<SuMenu>().Include(i => i.MenuLabels).FirstOrDefaultAsync(i => i.MenuCode == request.MenuCode );
                _context.Entry(menu).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<SuMenu>().Remove(menu);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
