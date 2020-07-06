using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT10
{
    public class Delete
    {
        public class Command : ICommand
        {
            public string EducationTypeCode { get; set; }
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
                var educationtype = new DbEducationType { CompanyCode = _user.Company, EducationTypeCode = request.EducationTypeCode, RowVersion = request.RowVersion };
                _context.Set<DbEducationType>().Attach(educationtype);
                _context.Set<DbEducationType>().Remove(educationtype);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
