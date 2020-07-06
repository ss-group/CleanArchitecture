using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT09
{
    public class Delete
    {
        public class Command : ICommand
        {
            public string EducationTypeLevel { get; set; }

            public string CompanyCode { get; set; }
            public uint RowVersion { get; set; }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context ,ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var educationtype = new DbEducationTypeLevel { CompanyCode = _user.Company ,EducationTypeLevel = request.EducationTypeLevel, RowVersion = request.RowVersion };
                _context.Set<DbEducationTypeLevel>().Attach(educationtype);
                _context.Set<DbEducationTypeLevel>().Remove(educationtype);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
