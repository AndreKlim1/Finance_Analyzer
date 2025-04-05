namespace IntegrationService.Services
{
    using System.Net.Http.Json;
    using System.Globalization;
    using IntegrationService.DTO;

    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly HttpClient _httpClient;
        private const string RatesUrl = "https://cdn.jsdelivr.net/npm/@fawazahmed0/currency-api@latest/v1/currencies/eur.json";

        public CurrencyConversionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CurrencyRates> GetLatestRatesAsync(CancellationToken cancellationToken)
        {
            var ratesResponse = await _httpClient.GetFromJsonAsync<CurrencyRates>(RatesUrl, cancellationToken);
            if (ratesResponse == null || ratesResponse.Rates == null || ratesResponse.Rates.Count == 0)
                throw new Exception("Не удалось получить курсы валют.");

            ratesResponse.Rates = ratesResponse.Rates.ToDictionary(
                kvp => kvp.Key.ToUpperInvariant(),
                kvp => kvp.Value);
            return ratesResponse;
        }

        public async Task<decimal> ConvertCurrencyAsync(string targetCurrency, string transactionCurrency, decimal transactionValue, CancellationToken cancellationToken)
        {

            targetCurrency = targetCurrency.ToUpperInvariant();
            transactionCurrency = transactionCurrency.ToUpperInvariant();

            if (targetCurrency == transactionCurrency)
                return transactionValue;

            var ratesResponse = await GetLatestRatesAsync(cancellationToken);
            var rates = ratesResponse.Rates;

            decimal rateTarget = targetCurrency == "EUR" ? 1m : (rates.ContainsKey(targetCurrency) ? rates[targetCurrency] : throw new Exception($"Курс для валюты {targetCurrency} не найден."));
            decimal rateTransaction = transactionCurrency == "EUR" ? 1m : (rates.ContainsKey(transactionCurrency) ? rates[transactionCurrency] : throw new Exception($"Курс для валюты {transactionCurrency} не найден."));


            var valueInEur = transactionValue / rateTransaction;
            var convertedValue = valueInEur * rateTarget;

            return convertedValue;
        }
    }

}
