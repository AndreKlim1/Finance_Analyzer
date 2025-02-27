using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CaregoryAccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace CaregoryAccountService.Repositories
{
    public abstract class RepositoryBase<T, TKey>(CategoryAccountServiceDbContext dbContext)
    : IRepositoryBase<T, TKey> where T : BaseModel<TKey> where TKey : struct
    {
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken token)
        {
            return await dbContext.FindAsync<T>(id, token);
        }

        public virtual async Task AddAsync(T entity, CancellationToken token)
        {
            await dbContext.AddAsync(entity, token);
            await dbContext.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken token)
        {
            var ent = await dbContext.FindAsync<T>(id, token);
            if (ent != null)
            {
                dbContext.Remove(ent);
                await dbContext.SaveChangesAsync(token);
            }
        }

        public virtual async Task<T?> UpdateAsync(T entity, CancellationToken token)
        {
            var ent = await dbContext.FindAsync<T>(entity.Id, token);
            if (ent == null) return null;
            dbContext.Entry(ent).CurrentValues.SetValues(entity);
            await dbContext.SaveChangesAsync(token);
            return entity;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? dbContext.Set<T>().AsNoTracking() : dbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? dbContext.Set<T>().Where(expression).AsNoTracking() : dbContext.Set<T>().Where(expression);

        public void Add(T entity) => dbContext.Set<T>().AddAsync(entity);
        public void Update(T entity) => dbContext.Set<T>().Update(entity);
        public void Delete(T entity) => dbContext.Set<T>().Remove(entity);
    }
}
