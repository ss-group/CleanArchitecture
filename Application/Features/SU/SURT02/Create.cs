using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Create
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
                if (_context.Set<SuProgram>().Any(o => o.ProgramCode.ToUpper() == request.ProgramCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT02.ProgramCode");
                }

                foreach (SuProgramLabel item in request.ProgramLabels)
                {
                    item.SystemCode = request.SystemCode;
                    item.ModuleCode = request.ModuleCode;
                }

                _context.Set<SuProgram>().Add((SuProgram)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
