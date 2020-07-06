using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT05
{
    public class Edit
    {
        public class Command : SuParameter, ICommand
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
                _context.Set<SuParameter>().Attach((SuParameter)request);
                _context.Entry((SuParameter)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
