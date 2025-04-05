using IntegrationService.DTO;

namespace IntegrationService.Messaging.Http
{
    public class TransactionClient : ITransactionClient
    {
        private readonly HttpClient _httpClient;

        public TransactionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateTransactionAsync(CreateTransactionRequest transaction, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("http://transactionsservice:8080/api/v1.0/transactions", transaction, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
