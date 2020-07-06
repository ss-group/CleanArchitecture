using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class DeleteCompany
    {
        public class Command : SuCompany, ICommand
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
                var company = await _context.Set<SuCompany>().Include(i => i.Divisions).ThenInclude(i => i.Divisions).FirstOrDefaultAsync(i => i.CompanyCode == request.CompanyCode);
                _context.Entry(company).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<SuCompany>().Remove(company);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
