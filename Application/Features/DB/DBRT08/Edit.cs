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

namespace Application.Features.DB.DBRT08
{
    public class Edit
    {
        public class Command : DbEmployee, ICommand
        {

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly EmployeeService _es;
            public Handler(ICleanDbContext context,EmployeeService es)
            {
                _context = context;
                _es = es;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbEmployee>().Attach((DbEmployee)request);
                request.tNameConcat = await _es.GetConcatNameTha(request.PreNameId, request.tFirstName, request.tLastName);
                request.eNameConcat = await _es.GetConcatNameEng(request.PreNameId, request.tFirstName, request.tLastName);
                _context.Entry((DbEmployee)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
