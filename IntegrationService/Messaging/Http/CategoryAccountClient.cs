using IntegrationService.DTO;

namespace IntegrationService.Messaging.Http
{
    public class CategoryAccountClient : ICategoryAccountClient
    {
        private readonly HttpClient _httpClient;

        public CategoryAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryNameDTO>> GetCategoriesAsync(long userId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<List<CategoryNameDTO>>($"http://categoryaccountservice:8080/api/v1.0/categories/user/{userId}", cancellationToken);
            return response ?? new List<CategoryNameDTO>();
        }

        public async Task<CategoryNameDTO> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("http://categoryaccountservice:8080/api/v1.0/categories", createCategoryRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategoryNameDTO>(cancellationToken: cancellationToken);
        }

        public async Task<List<AccountNameDTO>> GetAccountsAsync(long userId, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<List<AccountNameDTO>>($"http://categoryaccountservice:8080/api/v1.0/accounts/user/{userId}", cancellationToken);
            return response ?? new List<AccountNameDTO>();
        }

        public async Task<AccountNameDTO> CreateAccountAsync(CreateAccountRequest createAccountRequest, CancellationToken cancellationToken)
        {

            var response = await _httpClient.PostAsJsonAsync("http://categoryaccountservice:8080/api/v1.0/accounts", createAccountRequest, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AccountNameDTO>(cancellationToken: cancellationToken);
        }
    }
}
