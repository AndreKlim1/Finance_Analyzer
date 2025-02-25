using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models.DTO.Requests
{
    public record UpdateCategoryRequest(
        long Id,
        long UserId,
        string CategoryName,
        string CategoryType);
}
