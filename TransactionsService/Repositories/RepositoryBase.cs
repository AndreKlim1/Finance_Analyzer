using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TransactionsService.Models;

namespace TransactionsService.Repositories
{
    public abstract class RepositoryBase<T, TKey>(TransactionsServiceDbContext DbContext)
    : IRepositoryBase<T, TKey> where T : BaseModel<TKey> where TKey : struct
    {
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken token)
        {
            return await DbContext.FindAsync<T>(id, token);
        }

        public virtual async Task AddAsync(T entity, CancellationToken token)
        {
            await DbContext.AddAsync(entity, token);
            await DbContext.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken token)
        {
            var ent = await DbContext.FindAsync<T>(id, token);
            if (ent != null)
            {
                DbContext.Remove(ent);
                await DbContext.SaveChangesAsync(token);
            }
        }

        public virtual async Task<T?> UpdateAsync(T entity, CancellationToken token)
        {
            var ent = await DbContext.FindAsync<T>(entity.Id, token);
            if (ent == null) return null;
            DbContext.Entry(ent).CurrentValues.SetValues(entity);
            await DbContext.SaveChangesAsync(token);
            return entity;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? DbContext.Set<T>().AsNoTracking() : DbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? DbContext.Set<T>().Where(expression).AsNoTracking() : DbContext.Set<T>().Where(expression);

        public void Add(T entity) => DbContext.Set<T>().AddAsync(entity);
        public void Update(T entity) => DbContext.Set<T>().Update(entity);
        public void Delete(T entity) => DbContext.Set<T>().Remove(entity);
    }
}
