using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT03
{
    public class Detail
    {
        public class Query : IRequest<SuMenu>
        {
            public string MenuCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuMenu>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }

            public async Task<SuMenu> Handle(Query request, CancellationToken cancellationToken)
            {
                var menu = await _context.Set<SuMenu>().Where(i => i.MenuCode == request.MenuCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                
                if (menu == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }

                menu.MenuLabels = await _context.Set<SuMenuLabel>().Where(i => i.MenuCode == request.MenuCode).AsNoTracking().ToListAsync(cancellationToken);

                return menu;
            }
        }
    }
}
