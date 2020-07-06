using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT01
{
    public class DetailDivision
    {
        public class Query : IRequest<SuDivision>
        {
            public string CompanyCode { get; set; }
            public string DivCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuDivision>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuDivision> Handle(Query request, CancellationToken cancellationToken)
            {
                var division = await _context.Set<SuDivision>().Where(i => i.CompanyCode == request.CompanyCode && i.DivCode == request.DivCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (division == null)
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");

                return division;
            }
        }
    }
}
