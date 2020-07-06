using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT07
{
    public class Create
    {
        public class Command : SuPasswordPolicy, ICommand
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
                if (_context.Set<SuPasswordPolicy>().Any(i => i.PasswordPolicyCode.ToUpper() == request.PasswordPolicyCode.ToUpper()))
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT07.PasswordPolicyCode");

                _context.Set<SuPasswordPolicy>().Add((SuPasswordPolicy)request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
