using IntegrationService.DTO;
using System.Threading.Tasks;

namespace IntegrationService.Services
{
    public interface ICurrencyConversionService
    {
        Task<CurrencyRates> GetLatestRatesAsync(CancellationToken cancellationToken);
        Task<decimal> ConvertCurrencyAsync(string budgetCurrency, string transactionCurrency, decimal transactionValue, CancellationToken cancellationToken);
    }
}
