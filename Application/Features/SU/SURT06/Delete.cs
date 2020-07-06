using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class Delete
    {
        public class Command : ICommand
        {
            public long Id { get; set; }

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
               var user = new SuUser {  Id = request.Id,RowVersion = request.RowVersion };
                user.UserType = await _context.Set<SuUserType>().FirstOrDefaultAsync(o => o.UserId == request.Id, cancellationToken);
                _context.Set<SuUser>().Attach(user);
                _context.Set<SuUser>().Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
