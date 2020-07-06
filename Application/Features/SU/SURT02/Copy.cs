using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using Domain.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Copy
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
                request.ProgramLabels.Clear();
                var programLabels = await _context.Set<SuProgramLabel>().Where(i => i.ProgramCode == request.ProgramCode && i.LangCode == Lang.th).AsNoTracking().ToListAsync(cancellationToken);

                foreach (SuProgramLabel item in programLabels)
                {
                    request.ProgramLabels.Add(new SuProgramLabel { RowState = RowState.Add, FieldName = item.FieldName, LangCode = Lang.en, LabelName = item.LabelName, SystemCode = item.SystemCode, ModuleCode = item.ModuleCode });
                }

                _context.Set<SuProgram>().Attach((SuProgram)request);
                _context.Entry((SuProgram)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
