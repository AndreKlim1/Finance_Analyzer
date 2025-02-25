using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models.DTO.Requests
{
    public record CreateCategoryRequest(
        long UserId,
        string CategoryName,
        string CategoryType);
}
