using CategoryAccountService.Messaging.DTO;

namespace CategoryAccountService.Messaging.Http
{
    public class CurrencyConversionClient : ICurrencyConversionClient
    {
        private readonly HttpClient _httpClient;

        public CurrencyConversionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertTransactionValueAsync(string targetCurrency, string transactionCurrency, decimal transactionValue)
        {
            var request = new CurrencyConversionRequest
            {
                TargetCurrency = targetCurrency,
                TransactionCurrency = transactionCurrency,
                TransactionValue = transactionValue
            };

            var response = await _httpClient.PostAsJsonAsync("http://integrationservice:8080/api/v1.0/conversion/convert", request);
            response.EnsureSuccessStatusCode();

            var conversionResponse = await response.Content.ReadFromJsonAsync<CurrencyConversionResponse>();
            if (conversionResponse == null)
                throw new Exception("Получен пустой ответ от сервиса конвертации.");

            return conversionResponse.ConvertedValue;
        }
    }
}
