using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Infra.Data.MongoDB.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.MongoDB.Cache
{
    public class SaleItemCache : ISaleItemCache
    {
        private readonly MongoDbContext _mongoDbContext;

        public SaleItemCache(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
        }

        public async Task AddAsync(SaleItem entity)
        {
            await _mongoDbContext.SaleItems.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(SaleItem entity)
        {
            var filter = Builders<SaleItem>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.SaleItems.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(SaleItem entity)
        {
            var filter = Builders<SaleItem>.Filter.Eq(c => c.Id, entity.Id);
            await _mongoDbContext.SaleItems.DeleteOneAsync(filter);
        }

        public async Task<List<SaleItem>> GetAllAsync()
        {
            return await _mongoDbContext.SaleItems.Find(FilterDefinition<SaleItem>.Empty).ToListAsync();
        }

        public async Task<SaleItem?> GetByIdAsync(string id)
        {
            var filter = Builders<SaleItem>.Filter.Eq(c => c.Id, id);
            return await _mongoDbContext.SaleItems.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<SaleItem>> GetAllWithFilterAsync(FilterDefinition<SaleItem> filter)
        {
            return await _mongoDbContext.SaleItems.Find(filter).ToListAsync();
        }

        public async Task<SaleItem?> GetWithFilterAsync(FilterDefinition<SaleItem> filter)
        {
            return await _mongoDbContext.SaleItems.Find(filter).FirstOrDefaultAsync();
        }
    }
}
