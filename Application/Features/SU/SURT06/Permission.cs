using Application.Common.Mapping;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class Permission
    {
        public class Query : IRequest<SuUserPermission>
        {
            public long Id { get; set; }
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuUserPermission>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuUserPermission> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Set<SuUserPermission>().Include(o=>o.Divisions).Include(o=>o.EduLevels)
                .FirstOrDefaultAsync(o=>o.Id == request.Id && o.CompanyCode == request.CompanyCode,cancellationToken);
            }
        }
    }
}
