using Application.Common.Behaviors;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Features.Services;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class CreatePermission
    {
        public class Command : SuUserPermission, ICommand
        {
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            private readonly IIdentityService _identity;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user, IIdentityService identity)
            {
                _context = context;
                _user = user;
                _identity = identity;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Set<SuUserPermission>().AnyAsync(o => o.Id == request.Id && o.CompanyCode == request.CompanyCode, cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.SURT06.CompanyCode");
                }

                _context.Set<SuUserPermission>().Add(request);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }

    }
}
