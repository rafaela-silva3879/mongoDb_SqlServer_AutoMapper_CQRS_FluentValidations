using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    : IBaseRepository<User, string>
    {
    }
}
