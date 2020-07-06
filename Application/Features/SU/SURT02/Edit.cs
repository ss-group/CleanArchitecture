using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Edit
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
                _context.Set<SuProgramLabel>().RemoveRange(request.ProgramLabels.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.ProgramLabels = request.ProgramLabels.Where(o => o.RowState != RowState.Delete).ToList();

                foreach (var room in request.ProgramLabels.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<SuProgramLabel>().Attach(room);
                    _context.Entry(room).State = EntityState.Modified;
                }

                _context.Set<SuProgram>().Attach((SuProgram)request);
                _context.Entry((SuProgram)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
