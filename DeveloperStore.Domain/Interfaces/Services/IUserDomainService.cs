using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Interfaces.Services
{
    public interface IUserDomainService : IBaseDomainService<User, string>
    {
        Task<User> ValidateCredentialsAsync(string email, string senha);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByTokenAsync(string token);
    }
}
