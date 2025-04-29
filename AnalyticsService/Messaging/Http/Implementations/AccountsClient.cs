using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;

namespace AnalyticsService.Messaging.Http.Implementations
{
    public class AccountsClient : IAccountsClient
    {
        private readonly HttpClient _httpClient;

        public AccountsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AccountResponse> GetAccountByIdAsync(long accountId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<AccountResponse>($"http://categoryaccountservice:8080/api/v1.0/accounts/{accountId}", cancellationToken);
            return response;
        }
    }
}
