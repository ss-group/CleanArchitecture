﻿using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT03
{
    public class Create
    {
        public class Command : DbProvince, ICommand<long?>
        {

        }

        public class Handler : IRequestHandler<Command, long?>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<long?> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Set<DbProvince>().Add((DbProvince)request);
                await _context.SaveChangesAsync(cancellationToken);
                return request.ProvinceId;
            }
        }
    }
}
