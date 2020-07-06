using Application.Common.Mapping;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.DB;
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
    public class Detail
    {
        public class UserVm : SuUser, IMapFrom<SuUser>
        {
            public new IEnumerable<UserPermissionVm> Permissions {get;set;}
            public void Mapping(Profile profile)
            {
                profile.CreateMap<SuUser, UserVm>();
            }
        }

        public class UserPermissionVm : SuUserPermission ,IMapFrom<SuUserPermission>{
            public string CompanyName {get;set;}
            public void Mapping(Profile profile)
            {
                profile.CreateMap<SuUserPermission, UserPermissionVm>();
            }
        }
        public class Query : IRequest<UserVm>
        {
            public long Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserVm>
        {
            private readonly ICleanDbContext _context;
            private readonly ICurrentUserAccessor _user;
            private readonly IMapper _mapper;
            public Handler(ICleanDbContext context, ICurrentUserAccessor user, IMapper mapper)
            {
                _context = context;
                _user = user;
                _mapper = mapper;
            }
            public async Task<UserVm> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Set<SuUser>().Where(o => o.Id == request.Id).ProjectTo<UserVm>(_mapper.ConfigurationProvider).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "message.NotFound");
                }
  
                user.Profiles = user.Profiles.OrderBy(o => o.ProfileCode).ToList();
                user.PasswordHash = null;

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT p.user_id as id");
                sql.AppendLine(" ,p.company_code as companyCode ");
                sql.AppendLine(" ,case 'th' when 'th' then c.company_name_tha else coalesce(c.company_name_eng,c.company_name_tha ) end as companyName");
                sql.AppendLine("  ,is_default  as isDefault ");
                sql.AppendLine("  ,p.xmin as rowVersion");
                sql.AppendLine("  from su_user_permission p ");
                sql.AppendLine("  inner join su_company c on c.company_code  = p.company_code ");
                sql.AppendLine("  where p.user_id = @Id ");
                user.Permissions = await _context.QueryAsync<UserPermissionVm>(sql.ToString(),new { Id = user.Id},cancellationToken);
                return user;
            }
        }
    }
}
