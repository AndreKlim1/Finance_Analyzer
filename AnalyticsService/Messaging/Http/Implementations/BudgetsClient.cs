using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;


namespace AnalyticsService.Messaging.Http.Implementations
{
    public class BudgetsClient : IBudgetsClient
    {
        private readonly HttpClient _httpClient;

        public BudgetsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BudgetResponse> GetBudgetByIdAsync(long budgetId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<BudgetResponse>($"http://budgetingservice:8080/api/v1.0/budgets/{budgetId}", cancellationToken);
            return response;
        }
    }
}
