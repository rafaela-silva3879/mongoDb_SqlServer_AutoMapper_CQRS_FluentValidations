using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
namespace DeveloperStore.Domain.Interfaces.Cache
{
    public interface IBaseCache<TEntity, TKey>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(TKey id);
        //FilterDefinition cannot be found
        Task<List<TEntity>> GetAllWithFilterAsync(FilterDefinition<TEntity> filter);
        Task<TEntity> GetWithFilterAsync(FilterDefinition<TEntity> filter);
    }
}
