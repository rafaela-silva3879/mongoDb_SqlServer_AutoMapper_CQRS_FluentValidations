using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace DeveloperStore.Infra.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {
        private readonly DataContext _dataContext;

        protected BaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual async Task<TKey> AddAsync(TEntity entity)
        {
            _dataContext.Entry(entity).State = EntityState.Added;
            await _dataContext.SaveChangesAsync();

            var keyProperty = typeof(TEntity).GetProperty("Id");
            if (keyProperty == null)
                throw new Exception("The entity does not have a prerty 'Id'.");

            return (TKey)Convert.ChangeType(keyProperty.GetValue(entity), typeof(TKey));
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dataContext.Entry(entity).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _dataContext.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dataContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dataContext.Set<TEntity>().Where(where).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dataContext.Set<TEntity>().FirstOrDefaultAsync(where);
        }

        public virtual void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}