using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT11
{
    public class Delete
    {
        public class Command : ICommand
        {

            public string FacCode { get; set; }

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
                var factype = new DbFac { CompanyCode = _user.Company, FacCode = request.FacCode, RowVersion = request.RowVersion };

                _context.Set<DbFac>().Attach(factype);
                _context.Set<DbFac>().Remove(factype);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
