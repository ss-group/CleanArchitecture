using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT03
{
    public class Detail
    {
        public class Query : IRequest<DbProvince>
        {
            public string ProvinceCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbProvince>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbProvince> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Set<DbProvince>().Where(o => o.ProvinceCode == request.ProvinceCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
                return result;
            }


        }
    }
}
