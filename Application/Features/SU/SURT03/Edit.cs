using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
{
    public class Edit
    {
        public class Command : SuMenu, ICommand
        {

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<SuMenuLabel>().RemoveRange(request.MenuLabels.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.MenuLabels = request.MenuLabels.Where(o => o.RowState != RowState.Delete).ToList();

                foreach (var label in request.MenuLabels.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<SuMenuLabel>().Attach(label);
                    _context.Entry(label).State = EntityState.Modified;
                }

                _context.Set<SuMenu>().Attach((SuMenu)request);
                _context.Entry((SuMenu)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
