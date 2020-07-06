using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.DB;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.DB.DBRT18
{
    public class Manage
    {
        public class Query : IRequest<DbListItem>
        {
            public string ListItemGroupCode { get; set; }
            public string ListItemCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, DbListItem>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<DbListItem> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _context.Set<DbListItem>().Where(o => o.ListItemGroupCode ==  request.ListItemGroupCode && o.ListItemCode == request.ListItemCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (result == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Message.NotFound");
                }
                return result;
            }


        }
    }
}
