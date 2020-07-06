using Application.Common.Exceptions;
using Application.Common.Interfaces;
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

namespace Application.Features.SU.SURT05
{
    public class Detail
    {
        public class Query : IRequest<SuParameter>
        {
            public string ParameterGroupCode { get; set; }
            public string ParameterCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuParameter>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuParameter> Handle(Query request, CancellationToken cancellationToken)
            {
                var parameter = await _context.Set<SuParameter>().Where(i => i.ParameterGroupCode == request.ParameterGroupCode && i.ParameterCode == request.ParameterCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (parameter == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return parameter;
            }
        }
    }
}
