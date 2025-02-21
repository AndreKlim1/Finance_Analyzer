using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public abstract class RepositoryBase<T, TKey>(BookShopDbContext bookShopDbContext)
    : IRepositoryBase<T, TKey> where T : BaseModel<TKey> where TKey : struct
    {
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken token)
        {
            return await bookShopDbContext.FindAsync<T>(id, token);
        }

        public virtual async Task AddAsync(T entity, CancellationToken token)
        {
            await bookShopDbContext.AddAsync(entity, token);
            await bookShopDbContext.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken token)
        {
            var ent = await bookShopDbContext.FindAsync<T>(id, token);
            if (ent != null)
            {
                bookShopDbContext.Remove(ent);
                await bookShopDbContext.SaveChangesAsync(token);
            }
        }

        public virtual async Task<T?> UpdateAsync(T entity, CancellationToken token)
        {
            var ent = await bookShopDbContext.FindAsync<T>(entity.Id, token);
            if (ent == null) return null;
            bookShopDbContext.Update(entity);
            await bookShopDbContext.SaveChangesAsync(token);
            return entity;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? bookShopDbContext.Set<T>().AsNoTracking() : bookShopDbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? bookShopDbContext.Set<T>().Where(expression).AsNoTracking() : bookShopDbContext.Set<T>().Where(expression);

        public void Add(T entity) => bookShopDbContext.Set<T>().AddAsync(entity);
        public void Update(T entity) => bookShopDbContext.Set<T>().Update(entity);
        public void Delete(T entity) => bookShopDbContext.Set<T>().Remove(entity);
    }
}
