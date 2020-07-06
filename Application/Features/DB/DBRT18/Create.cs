using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT18
{
    public class Create
    {
        public class Command : DbListItem, ICommand
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
                if (_context.Set<DbListItem>().Any(o => o.ListItemGroupCode == request.ListItemGroupCode && o.ListItemCode == request.ListItemCode))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT18.listItemCode");
                }

                _context.Set<DbListItem>().Add((DbListItem)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

        }
    }
}
