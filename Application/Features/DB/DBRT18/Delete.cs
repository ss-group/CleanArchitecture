using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT18
{
    public class Delete
    {
        public class Command : ICommand
        {
            public string ListItemGroupCode { get; set; }
            public string ListItemCode { get; set; }
            public uint RowVersion { get; set; }

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _user = user;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var listitem = new DbListItem { ListItemGroupCode = request.ListItemGroupCode, ListItemCode = request.ListItemCode, RowVersion = request.RowVersion };

                _context.Set<DbListItem>().Attach(listitem);
                _context.Set<DbListItem>().Remove(listitem);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
