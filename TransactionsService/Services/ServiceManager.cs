using TransactionsService.Services.Interfaces;



namespace TransactionsService.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly ITransactionService _transactionService;

        public ServiceManager(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            
        }

    }
}
