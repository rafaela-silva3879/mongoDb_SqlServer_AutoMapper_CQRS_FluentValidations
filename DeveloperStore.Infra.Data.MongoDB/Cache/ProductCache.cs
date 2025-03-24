using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Infra.Data.MongoDB.Contexts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.MongoDB.Cache
{
    public class ProductCache : IProductCache
    {
        private readonly IMongoCollection<Product> _products;

        public ProductCache(MongoDbContext mongoDbContext)
        {
            if (mongoDbContext == null) throw new ArgumentNullException(nameof(mongoDbContext));
            _products = mongoDbContext.Products;
        }

        public async Task AddAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _products.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var filter = Builders<Product>.Filter.Eq(p => p.Id, entity.Id);
            await _products.ReplaceOneAsync(filter, entity, new ReplaceOptions { IsUpsert = true });
        }

        public async Task DeleteAsync(Product entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var filter = Builders<Product>.Filter.Eq(p => p.Id, entity.Id);
            await _products.DeleteOneAsync(filter);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _products.Find(FilterDefinition<Product>.Empty).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllWithFilterAsync(FilterDefinition<Product> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<Product?> GetWithFilterAsync(FilterDefinition<Product> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            return await _products.Find(filter).FirstOrDefaultAsync();
        }
    }
}
