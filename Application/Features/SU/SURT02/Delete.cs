using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Delete
    {
        public class Command : SuProgram, ICommand
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
                var program = await _context.Set<SuProgram>().Include(i => i.ProgramLabels).FirstOrDefaultAsync(i => i.ProgramCode == request.ProgramCode);
                _context.Entry(program).Property("RowVersion").OriginalValue = request.RowVersion;
                _context.Set<SuProgram>().Remove(program);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
