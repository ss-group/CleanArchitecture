using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT14
{
    public class Delete
    {
        public class Command : ICommand
        {
            public int ProjectId { get; set; }
            public string ProjectCode { get; set; }
            public uint RowVersion { get; set; }

        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _user = user;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var project = new DbProject { ProjectId = request.ProjectId, ProjectCode = request.ProjectCode, RowVersion = request.RowVersion };

                _context.Set<DbProject>().Attach(project);
                _context.Set<DbProject>().Remove(project);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
