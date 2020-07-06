using Application.Common.Models;
using Domain.Entities.SU;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(long userId);

        Task<(Result Result, long UserId)> CreateUserAsync(SuUser user, string password);

        Task<Result> DeleteUserAsync(long userId);

        Task<Result> UpdateUserAsync(SuUser user);
        string GeneratePassword();
    }
}
