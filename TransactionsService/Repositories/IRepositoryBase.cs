using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionsService.Models;
using TransactionsService.Models;

namespace TransactionsService.Repositories
{
    public interface IRepositoryBase<T, in TKey> where T : BaseModel<TKey> where TKey : struct
    {
        Task<T?> GetByIdAsync(TKey id, CancellationToken token);
        Task AddAsync(T entity, CancellationToken token);
        Task<T?> UpdateAsync(T entity, CancellationToken token);
        Task DeleteAsync(TKey id, CancellationToken token);

        /// <summary> Выборка по всем записям из таблицы </summary>
        /// <param name="trackChanges">признак отслеживания</param>
        /// <returns></returns>
        IQueryable<T> FindAll(bool trackChanges);

        /// <summary> Выборка записей из таблицы по условию </summary>
        /// <param name="expression">условие</param>
        /// <param name="trackChanges">признак отслеживания</param>
        /// <returns></returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        /// <summary> Синхронная версия метода создания записи </summary>
        void Add(T entity);
        /// <summary> Синхронная версия метода обновления записи </summary>
        void Update(T entity);
        /// <summary> Синхронная версия метода удаления записи </summary>
        void Delete(T entity);
    }
}
