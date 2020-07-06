using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT07
{
    public class Delete
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
                SuPasswordPolicy passwordPolicy = new SuPasswordPolicy { PasswordPolicyCode = request.PasswordPolicyCode, RowVersion = request.RowVersion };
                _context.Set<SuPasswordPolicy>().Attach(passwordPolicy);
                _context.Set<SuPasswordPolicy>().Remove(passwordPolicy);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
