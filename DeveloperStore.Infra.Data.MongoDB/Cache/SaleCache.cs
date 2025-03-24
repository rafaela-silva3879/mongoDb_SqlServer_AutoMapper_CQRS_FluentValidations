using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Infra.Data.MongoDB.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.MongoDB.Cache
{
    public class SaleCache : ISaleCache
    {
        private readonly MongoDbContext _mongoDbContext;

        public SaleCache(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
        }

        public async Task AddAsync(Sale entity)
        {
            await _mongoDbContext.Sales.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Sale entity)
        {
            var filter = Builders<Sale>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.Sales.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Sale entity)
        {
            var filter = Builders<Sale>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.Sales.DeleteOneAsync(filter);
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _mongoDbContext.Sales.Find(FilterDefinition<Sale>.Empty).ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(string id)
        {
            var filter = Builders<Sale>.Filter.Eq(c => c.Id, id);
            return await _mongoDbContext.Sales.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Sale>> GetAllWithFilterAsync(FilterDefinition<Sale> filter)
        {
            return await _mongoDbContext.Sales.Find(filter).ToListAsync();
        }

        public async Task<Sale?> GetWithFilterAsync(FilterDefinition<Sale> filter)
        {
            return await _mongoDbContext.Sales.Find(filter).FirstOrDefaultAsync();
        }
    }
}
