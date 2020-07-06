using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
{
    public class Create
    {
        public class Command : SuMenu, ICommand
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
                if (_context.Set<SuMenu>().Any(i => i.MenuCode.ToUpper() == request.MenuCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT03.MenuCode");
                }

                foreach (SuMenuLabel menuLabel in request.MenuLabels)
                {
                    menuLabel.SystemCode = request.SystemCode;
                }

                _context.Set<SuMenu>().Add((SuMenu)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
