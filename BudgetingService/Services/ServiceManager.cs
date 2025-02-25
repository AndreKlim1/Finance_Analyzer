using BudgetingService.Services.Interfaces;



namespace BudgetingService.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IBudgetService _transactionService;

        public ServiceManager(IBudgetService transactionService)
        {
            _transactionService = transactionService;
            
        }

    }
}
