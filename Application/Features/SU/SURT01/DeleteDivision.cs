using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class DeleteDivision
    {
        public class Command : SuDivision, ICommand
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
                var division = new SuDivision { CompanyCode = request.CompanyCode, DivCode = request.DivCode, RowVersion = request.RowVersion };
                _context.Set<SuDivision>().Attach(division);
                _context.Set<SuDivision>().Remove(division);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
