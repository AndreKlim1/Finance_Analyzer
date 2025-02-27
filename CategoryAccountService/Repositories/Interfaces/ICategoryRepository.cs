using CaregoryAccountService.Models;

namespace CaregoryAccountService.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category, long>
    {
        //public Task<Category> MergeCategoriesAsync(long sourceCategoryId, long targetCategoryId, CancellationToken token);
    }
}
