using AnalyticsService.Messaging.DTO;

namespace AnalyticsService.Messaging.Http.Interfaces
{
    public interface IAccountsClient
    {
        Task<AccountResponse> GetAccountByIdAsync(long accountId, CancellationToken cancellationToken);
    }
}
