using IntegrationService.DTO;

namespace IntegrationService.Messaging.Http
{
    public interface ITransactionClient
    {
        Task<bool> CreateTransactionAsync(CreateTransactionRequest transaction, CancellationToken cancellationToken);
    }
}
