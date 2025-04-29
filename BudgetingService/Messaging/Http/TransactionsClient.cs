using BudgetingService.Messaging.DTO;
using BudgetingService.Messaging.Http;
using BudgetingService.Models.Errors;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Net.Http;

namespace BudgetingService.Messaging.Http
{
    public class TransactionsClient : ITransactionsClient
    {
        private readonly HttpClient _httpClient;

        public TransactionsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<TransactionResponse>> GetTransactionsAsync(TransactionFilterParameters filter, CancellationToken cancellationToken)
        {
            var baseUrl = "http://transactionsservice:8080/api/v1.0/transactions/analytics";
            var queryParams = new Dictionary<string, string>
            {
                { "page", filter.Page.ToString() },
                { "userId", filter.UserId.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                queryParams.Add("searchTerm", filter.SearchTerm);
            }

            if (filter.StartDate.HasValue)
            {
                queryParams.Add("startDate", filter.StartDate.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
            }

            if (filter.EndDate.HasValue)
            {
                queryParams.Add("endDate", filter.EndDate.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(filter.Categories))
            {
                queryParams.Add("categories", filter.Categories);
            }

            if (!string.IsNullOrWhiteSpace(filter.Types))
            {
                queryParams.Add("types", filter.Types);
            }

            if (filter.MinValue.HasValue)
            {
                queryParams.Add("minValue", filter.MinValue.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (filter.MaxValue.HasValue)
            {
                queryParams.Add("maxValue", filter.MaxValue.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(filter.Accounts))
            {
                queryParams.Add("accounts", filter.Accounts);
            }

            var urlWithQuery = QueryHelpers.AddQueryString(baseUrl, queryParams);
            var response = await _httpClient.GetFromJsonAsync<List<TransactionResponse>>(urlWithQuery, cancellationToken);
            if (response is not null)
            {
                return response;
            }
            else
            {
                return new List<TransactionResponse>();
            }

        }
    }
}
