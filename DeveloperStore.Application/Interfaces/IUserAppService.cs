using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Interfaces
{
    public interface IUserAppService 
    {
        Task<UserQuery> CreateAsync(UserCreateCommand command);
        Task<UserQuery> UpdateAsync(UserUpdateCommand command);
        Task<UserQuery> DeleteAsync(UserDeleteCommand command);
        Task<UserQuery> GetByIdAsync(string id);
        Task<List<UserQuery>> GetAllAsync();
        Task<UserQuery> UpdatePrePasswordAsync(UserPreUpdatePasswordCommand command);
        Task<UserQuery> UpdatePasswordAsync(UserUpdatePasswordCommand command);
        Task<UserQuery> ValidateCredentialsAsync(UserValidateCredentialsCommand command);
        Task<UserQuery> GetUserByEmailAsync(UserGetUserByEmailCommand command);
        Task<UserQuery> GetUserByTokenAsync(UserGetUserByTokenCommand command);
     
    }
}
