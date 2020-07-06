using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using Domain.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class CheckDuplicate
    {
        public class Query : IRequest<bool>
        {
            public string ProgramCode { get; set; }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                List<SuProgramLabel> programLabel = await _context.Set<SuProgramLabel>().Where(i => i.ProgramCode == request.ProgramCode && i.LangCode == Lang.en).AsNoTracking().ToListAsync(cancellationToken);

                foreach (SuProgramLabel item in programLabel)
                {
                    if (_context.Set<SuProgramLabel>().Any(i => i.ProgramCode == item.ProgramCode && i.FieldName == item.FieldName && i.SystemCode == item.SystemCode && i.LangCode == Lang.th))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
