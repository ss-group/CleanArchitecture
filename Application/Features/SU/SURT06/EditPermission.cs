using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class EditPermission
    {
        public class Command : SuUserPermission, ICommand
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
                _context.Set<SuUserDivision>().RemoveRange(request.Divisions.Where(o=>o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                 request.Divisions =  request.Divisions.Where(o=>o.RowState == RowState.Add).ToList();

                _context.Set<SuUserEduLevel>().RemoveRange(request.EduLevels.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.EduLevels = request.EduLevels.Where(o => o.RowState == RowState.Add).ToList();

                _context.Set<SuUserPermission>().Attach((SuUserPermission)request);
                _context.Entry((SuUserPermission)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
