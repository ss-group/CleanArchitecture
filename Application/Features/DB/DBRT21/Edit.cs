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

namespace Application.Features.DB.DBRT21
{
    public class Edit
    {
        public class Command : DbFacProgram, ICommand
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
                _context.Set<DbFacProgramDetail>().RemoveRange(request.dbFacProgramDetail.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.dbFacProgramDetail = request.dbFacProgramDetail.Where(o => o.RowState != RowState.Delete).ToList();

                foreach (var group in request.dbFacProgramDetail.Where(o => o.RowState == RowState.Add))
                {
                      if (_context.Set<DbFacProgramDetail>().Any(o => o.CompanyCode == request.CompanyCode & o.FacCode == group.FacCode
                          & o.ProgramCode == group.ProgramCode & o.DepartmentCode == group.DepartmentCode))
                      {
                    throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT21.DepartmentCode");
                    }
                    _context.Set<DbFacProgramDetail>().Add(group);
                    _context.Entry(group).State = EntityState.Added;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                foreach (var group in request.dbFacProgramDetail.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<DbFacProgramDetail>().Attach(group);
                    _context.Entry(group).State = EntityState.Modified;
                }

                _context.Set<DbFacProgram>().Attach((DbFacProgram)request);
                _context.Entry((DbFacProgram)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

        }



    }
}
