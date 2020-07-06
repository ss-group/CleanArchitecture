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

namespace Application.Features.DB.DBRT13
{
    public class Edit
    {
        public class Command : DbMajor, ICommand
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
                _context.Set<DbProfessional>().RemoveRange(request.dbProfessional.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.dbProfessional = request.dbProfessional.Where(o => o.RowState != RowState.Delete).ToList();

                foreach (var group in request.dbProfessional.Where(o => o.RowState == RowState.Add))
                {

                    if (_context.Set<DbProfessional>().Any(o => o.CompanyCode == request.CompanyCode & o.MajorCode == group.MajorCode
                          & o.FacCode == group.FacCode & o.ProgramCode == group.ProgramCode & o.ProCode == group.ProCode))
                    {
                        throw new RestException(HttpStatusCode.BadRequest, "message.STD00004", "label.DBRT13.Professional");
                    }
  
                    _context.Set<DbProfessional>().Add(group);
                    _context.Entry(group).State = EntityState.Added;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                foreach (var group in request.dbProfessional.Where(o => o.RowState == RowState.Edit))
                {
                    _context.Set<DbProfessional>().Attach(group);
                    _context.Entry(group).State = EntityState.Modified;
                }

                _context.Set<DbMajor>().Attach((DbMajor)request);
                _context.Entry((DbMajor)request).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }

        }



    }
}
