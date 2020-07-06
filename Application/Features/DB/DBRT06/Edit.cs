using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT06
{
    public class Edit
    {
        public class Command : DbPostalCode, ICommand<long?>
        {

        }

        public class Handler : IRequestHandler<Command, long?>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context)
            {
                _context = context;
            }

            public async Task<long?> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbPostalCode>().Attach((DbPostalCode)request);
                _context.Entry((DbPostalCode)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return request.PostalCodeId;
            }
        }
    }
}
