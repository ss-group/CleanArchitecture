using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT04
{
    public class Delete
    {
        public class Command : SuProfile, ICommand
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
                var profile = await _context.Set<SuProfile>().Include(i => i.MenuProfiles).FirstOrDefaultAsync(i => i.ProfileCode == request.ProfileCode);
                _context.Entry(profile).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<SuProfile>().Remove(profile);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
