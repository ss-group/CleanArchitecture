using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT19
{
    public class Create
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
                _context.Set<DbDegree>().Add((DbDegree)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.DegreeId;
            }
        }
    }
}
