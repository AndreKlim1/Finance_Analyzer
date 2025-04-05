using CaregoryAccountService.Models.DTO.Requests;
using IntegrationService.DTO;

namespace IntegrationService.Messaging.Http
{
    public class CategoryNameDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class AccountNameDTO
    {
        public long Id { get; set; }
        public string AccountName { get; set; }
    }

    public interface ICategoryAccountClient
    {
        Task<List<CategoryNameDTO>> GetCategoriesAsync(long userId, CancellationToken cancellationToken);
        Task<CategoryNameDTO> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken);
        Task<List<AccountNameDTO>> GetAccountsAsync(long userId, CancellationToken cancellationToken);
        Task<AccountNameDTO> CreateAccountAsync(CreateAccountRequest createAccountRequest, CancellationToken cancellationToken);
    }
}
