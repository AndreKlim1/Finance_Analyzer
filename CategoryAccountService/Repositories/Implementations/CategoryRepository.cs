using CaregoryAccountService.Models;
using CaregoryAccountService.Repositories.Interfaces;

namespace CaregoryAccountService.Repositories.Implementations
{
    public class CategoryRepository : RepositoryBase<Category, long>, ICategoryRepository
    {
        private readonly CategoryAccountServiceDbContext _context;

        public CategoryRepository(CategoryAccountServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
