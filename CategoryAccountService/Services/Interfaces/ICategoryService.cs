using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;

namespace CaregoryAccountService.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> GetCategoryByIdAsync(long id, CancellationToken token);
        Task<Result<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken token);
        Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createProfileRequest, CancellationToken token);
        Task<Result<CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest updateProfileRequest, CancellationToken token);
        Task<bool> DeleteCategoryAsync(long id, CancellationToken token);
    }
}
