using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT04
{
    public class Copy
    {
        public class Command : ICommand
        {
            public string ProfileCodeFrom { get; set; }
            public string ProfileDescFrom { get; set; }
            public string ProfileCodeTo { get; set; }
            public string ProfileDescTo { get; set; }
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
                if (_context.Set<SuProfile>().Any(i => i.ProfileCode.ToUpper() == request.ProfileCodeTo.ToUpper()))
                    throw new RestException(HttpStatusCode.BadRequest, "message.SU00043");

                SuProfile suProfile = new SuProfile();
                suProfile.ProfileCode = request.ProfileCodeTo;
                suProfile.ProfileDesc = request.ProfileDescTo;
                suProfile.Active = true;
                _context.Set<SuProfile>().Add((SuProfile)suProfile);
                await _context.SaveChangesAsync(cancellationToken);

                List<SuMenuProfile> suMenuProfiles = await _context.Set<SuMenuProfile>().Where(e => e.ProfileCode == request.ProfileCodeFrom).AsNoTracking().ToListAsync(cancellationToken);
                foreach(SuMenuProfile suMenuProfile in suMenuProfiles)
                    suMenuProfile.ProfileCode = request.ProfileCodeTo;

                _context.Set<SuMenuProfile>().AddRange(suMenuProfiles);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
