using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class CreateDivision
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
                if (_context.Set<SuDivision>().Any(i => i.CompanyCode == request.CompanyCode && i.DivCode.ToUpper() == request.DivCode.ToUpper()))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT01.DivCode");
                }

                _context.Set<SuDivision>().Add((SuDivision)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
