using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Infra.Data.Contexts;
namespace DeveloperStore.Infra.Data.Repository
{
    public class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(DataContext dataContext)
            : base(dataContext)
        {
        }
    }
}