using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT16
{
    public class Delete
    {
        public class Command : ICommand
        {
            public string CompanyCode { get; set; }
            public string LocationCode { get; set; }
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
                var location = new DbLocation { CompanyCode = request.CompanyCode, LocationCode = request.LocationCode, RowVersion = request.RowVersion };

                _context.Set<DbLocation>().Attach(location);
                _context.Set<DbLocation>().Remove(location);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
