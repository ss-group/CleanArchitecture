using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
{
    public class Edit
    {
        public class Command : DbEducationType, ICommand
        {
            public class Handler : IRequestHandler<Command, Unit>
            {
                private readonly ICurrentUserAccessor _user;
                private readonly ICleanDbContext _context;
                public Handler(ICleanDbContext context, ICurrentUserAccessor user)
                {
                    _user = user;
                    _context = context;
                }

                public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                {
                    _context.Set<DbEducationType>().Attach((DbEducationType)request);
                    _context.Entry((DbEducationType)request).State = EntityState.Modified;
                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }
            }
        }
    }
}
