namespace AnalyticsService.Messaging.Http.Interfaces
{
    public interface ICurrencyConversionClient
    {
        Task<decimal> ConvertTransactionValueAsync(string targetCurrency, string transactionCurrency, decimal transactionValue);
    }
}
