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
    public abstract class RepositoryBase<T, TKey>(CategoryAccountServiceDbContext usersServiceDbContext)
    : IRepositoryBase<T, TKey> where T : BaseModel<TKey> where TKey : struct
    {
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken token)
        {
            return await usersServiceDbContext.FindAsync<T>(id, token);
        }

        public virtual async Task AddAsync(T entity, CancellationToken token)
        {
            await usersServiceDbContext.AddAsync(entity, token);
            await usersServiceDbContext.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken token)
        {
            var ent = await usersServiceDbContext.FindAsync<T>(id, token);
            if (ent != null)
            {
                usersServiceDbContext.Remove(ent);
                await usersServiceDbContext.SaveChangesAsync(token);
            }
        }

        public virtual async Task<T?> UpdateAsync(T entity, CancellationToken token)
        {
            var ent = await usersServiceDbContext.FindAsync<T>(entity.Id, token);
            if (ent == null) return null;
            usersServiceDbContext.Update(entity);
            await usersServiceDbContext.SaveChangesAsync(token);
            return entity;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? usersServiceDbContext.Set<T>().AsNoTracking() : usersServiceDbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? usersServiceDbContext.Set<T>().Where(expression).AsNoTracking() : usersServiceDbContext.Set<T>().Where(expression);

        public void Add(T entity) => usersServiceDbContext.Set<T>().AddAsync(entity);
        public void Update(T entity) => usersServiceDbContext.Set<T>().Update(entity);
        public void Delete(T entity) => usersServiceDbContext.Set<T>().Remove(entity);
    }
}
