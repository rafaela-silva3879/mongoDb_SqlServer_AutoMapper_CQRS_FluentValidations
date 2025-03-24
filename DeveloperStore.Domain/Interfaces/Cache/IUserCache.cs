using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Interfaces.Cache
{
    public interface IUserCache : IBaseCache<User, string>
    {
    }
}
