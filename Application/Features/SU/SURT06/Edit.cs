using Application.Common.Behaviors;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.SU;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SU.SURT06
{
    public class Edit
    {
        public class Command : SuUser, ICommand
        {
           public new string UserType { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICleanDbContext _context;
            private readonly IIdentityService _identity;
            private readonly ICurrentUserAccessor _user;
            public Handler(ICleanDbContext context,IIdentityService identity,ICurrentUserAccessor user)
            {
                _context = context;
                _identity = identity;
                _user = user;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Set<SuUser>().Where(o => o.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
                _context.Entry(user).Property("RowVersion").OriginalValue = request.RowVersion;
                user.ForceChangePassword = request.ForceChangePassword;
                user.Active = request.Active;
                user.DefaultLang = request.DefaultLang;
                user.PasswordPolicyCode = request.PasswordPolicyCode;
                user.StartEffectiveDate = request.StartEffectiveDate;
                user.EndEffectiveDate = request.EndEffectiveDate;
                user.UpdatedBy = _user.UserName;
                user.UpdatedDate = DateTime.Now;
                user.UpdatedProgram = _user.ProgramCode;
                var result = await _identity.UpdateUserAsync(user);
                if (!result.Succeeded)
                {
                    throw new RestException(System.Net.HttpStatusCode.InternalServerError,String.Join(",",result.Errors));
                }

                  _context.Set<SuUserProfile>().RemoveRange(request.Profiles.Where(o => o.RowState == RowState.Delete));
                await _context.SaveChangesAsync(cancellationToken);

                request.Profiles = request.Profiles.Where(o => o.RowState != RowState.Delete).ToList();
                
                _context.Set<SuUserProfile>().AddRange(request.Profiles);

                foreach(var permission in request.Permissions.Where(o => o.RowState == RowState.Delete))
                {
                     var userPermission = await _context.Set<SuUserPermission>().Include(g => g.Divisions).Include(g=>g.EduLevels)
                        .FirstOrDefaultAsync(g => g.Id == permission.Id && g.CompanyCode == permission.CompanyCode );
                    _context.Entry(userPermission).Property("RowVersion").OriginalValue = permission.RowVersion;
                    _context.Set<SuUserPermission>().Remove(userPermission);
                }
                
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
