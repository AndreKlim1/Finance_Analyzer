namespace CategoryAccountService.Messaging.Http
{
    public interface ICurrencyConversionClient
    {
        Task<decimal> ConvertTransactionValueAsync(string targetCurrency, string transactionCurrency, decimal transactionValue);
    }
}
