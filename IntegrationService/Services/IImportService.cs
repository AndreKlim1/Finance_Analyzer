using IntegrationService.DTO;

namespace IntegrationService.Services
{
    public interface IImportService
    {
        Task<ImportResult> ImportTransactionsAsync(IFormFile csvFile, long userId, CancellationToken cancellationToken);
        
    }
}
