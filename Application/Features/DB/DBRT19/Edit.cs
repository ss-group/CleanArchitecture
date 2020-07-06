using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT19
{
    public class Edit
    {
        public class Command : DbDegree, ICommand<long>
        {

        }
        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbDegree>().Attach((DbDegree)request);
                _context.Entry((DbDegree)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return request.DegreeId;
            }
        }
    }
}
