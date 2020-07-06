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

namespace Application.Features.DB.DBRT17
{
    public class Edit
    {
        public class Command : DbBuilding, ICommand<long>
        {

        }

        public class Handler : IRequestHandler<Command, long>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbRoom>().RemoveRange(request.dbRoom.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.dbRoom = request.dbRoom.Where(o => o.RowState != RowState.Delete).ToList();


                foreach (var group in request.dbRoom.Where(o => o.RowState == RowState.Add))
                {
                }
                foreach (var group in request.dbRoom.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<DbRoom>().Attach(group);
                    _context.Entry(group).State = EntityState.Modified;
                }

                _context.Set<DbPrivilegeBuilding>().RemoveRange(request.dbPrivilegeBuilding.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.dbPrivilegeBuilding = request.dbPrivilegeBuilding.Where(o => o.RowState != RowState.Delete).ToList();


                foreach (var group in request.dbPrivilegeBuilding.Where(o => o.RowState == RowState.Add))
                {

                }
                foreach (var group in request.dbPrivilegeBuilding.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<DbPrivilegeBuilding>().Attach(group);
                    _context.Entry(group).State = EntityState.Modified;
                }

                _context.Set<DbBuilding>().Attach((DbBuilding)request);
                _context.Entry((DbBuilding)request).State = EntityState.Modified;
                    
                await _context.SaveChangesAsync(cancellationToken);
                return request.BuildingId;
            }
           
        }



    }
}
