using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT04
{
    public class Edit
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
                _context.Set<SuMenuProfile>().RemoveRange(request.MenuProfiles.Where(i => i.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.MenuProfiles = request.MenuProfiles.Where(i => i.RowState != RowState.Delete).ToList();

                //foreach (var menu in request.MenuProfiles.Where(i => i.RowState == RowState.Normal))
                //{
                //    if (_context.Set<SuMenuProfile>().Any(i => i.MenuCode == menu.MenuCode))
                //        request.MenuProfiles.Remove(menu);
                //}

                _context.Set<SuProfile>().Attach((SuProfile)request);
                _context.Entry((SuProfile)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
