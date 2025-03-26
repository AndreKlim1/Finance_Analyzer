using CaregoryAccountService.Models.Enums;

namespace CaregoryAccountService.Models.DTO.Responses
{
    public record CategoryResponse(
        long Id,
        long? UserId,
        string CategoryName,
        string CategoryType,
        string Icon);
}
