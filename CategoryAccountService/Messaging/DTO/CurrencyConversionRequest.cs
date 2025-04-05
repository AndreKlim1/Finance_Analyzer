namespace CategoryAccountService.Messaging.DTO
{
    public class CurrencyConversionRequest
    {
        public string TargetCurrency { get; set; }
        public string TransactionCurrency { get; set; }
        public decimal TransactionValue { get; set; }
    }
}
