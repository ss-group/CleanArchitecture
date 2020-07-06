using Application.Common.Interfaces;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.Parameter
{
    public class Detail
    {
        public class Query : IRequest<JObject>
        {
            public string Group { get; set; }
            public string Code { get; set; }
        }
        public class Handler : IRequestHandler<Query, JObject>
        {
            private readonly ICleanDbContext _context;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user)
            {
                _context = context;
            }
            public async Task<JObject> Handle(Query request, CancellationToken cancellationToken)
            {
                var parameter = await _context.QuerySingleAsync<string>(@"select COALESCE(json_object_agg(sp.parameter_code, sp.parameter_value),'{ }') as parameter from su_parameter sp  where sp.parameter_group_code = @Group and sp.parameter_code = COALESCE(@Code,sp.parameter_code) ", new { Group = request.Group,Code = request.Code }, token: cancellationToken);
                return JObject.Parse(parameter);
            }
        }
    }
}
