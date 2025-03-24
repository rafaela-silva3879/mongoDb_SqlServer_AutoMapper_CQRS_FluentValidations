using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Infra.Data.MongoDB.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.MongoDB.Cache
{
    public class UserCache : IUserCache
    {
        private readonly MongoDbContext _mongoDbContext;

        public UserCache(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
        }

        public async Task AddAsync(User entity)
        {
            await _mongoDbContext.Users.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(User entity)
        {
            var filter = Builders<User>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.Users.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(User entity)
        {
            var filter = Builders<User>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.Users.DeleteOneAsync(filter);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _mongoDbContext.Users.Find(FilterDefinition<User>.Empty).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq(c => c.Id, id);
            return await _mongoDbContext.Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllWithFilterAsync(FilterDefinition<User> filter)
        {
            return await _mongoDbContext.Users.Find(filter).ToListAsync();
        }

        public async Task<User?> GetWithFilterAsync(FilterDefinition<User> filter)
        {
            return await _mongoDbContext.Users.Find(filter).FirstOrDefaultAsync();
        }
    }
}
