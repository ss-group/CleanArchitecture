using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.SU;
using Domain.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT02
{
    public class Detail
    {
        public class Query : IRequest<SuProgram>
        {
            public string ProgramCode { get; set; }
            public Lang Language { get; set; }
        }

        public class Handler : IRequestHandler<Query, SuProgram>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;

            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
                _user = user;
            }
            public async Task<SuProgram> Handle(Query request, CancellationToken cancellationToken)
            {
                var program = await _context.Set<SuProgram>().Where(o => o.ProgramCode == request.ProgramCode).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (program == null)
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");

                program.ProgramLabels = await _context.Set<SuProgramLabel>().Where(o => o.ProgramCode == request.ProgramCode && o.LangCode == request.Language).OrderBy(o => o.FieldName).AsNoTracking().ToListAsync(cancellationToken);

                return program;
            }
        }
    }
}
