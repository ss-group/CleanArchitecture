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
    public class DetailCompany
    {
        public class Query : IRequest<SuCompany>
        {
            public string CompanyCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuCompany>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuCompany> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = await _context.Set<SuCompany>().Where(i => i.CompanyCode == request.CompanyCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (company == null)
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");

                return company;
            }
        }
    }
}
