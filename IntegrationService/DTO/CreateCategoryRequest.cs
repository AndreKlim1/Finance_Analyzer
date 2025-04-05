
namespace IntegrationService.DTO
{
    public record CreateCategoryRequest(
        long? UserId,
        string CategoryName,
        string CategoryType,
        string Icon);
}
