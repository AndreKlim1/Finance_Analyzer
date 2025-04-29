using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Models.DTO.Response.AccountAnalytics;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations
{
    public class AccountAnalyticsService : IAccountAnalyticsService
    {
        private readonly ITransactionsClient _transactionsClient;
        private readonly IAccountsClient _accountsClient;
        private readonly ICurrencyConversionClient _currencyConversionClient;

        public AccountAnalyticsService(ITransactionsClient transactionsClient, IAccountsClient accountsClient, ICurrencyConversionClient currencyConversionClient)
        {
            _transactionsClient = transactionsClient;
            _accountsClient = accountsClient;
            _currencyConversionClient = currencyConversionClient;
        }

        public async Task<Result<AccountKpiResponse>> GetAccountKpiAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, accountId.ToString());

            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);
            var account = await _accountsClient.GetAccountByIdAsync(accountId, token);

            decimal inflow = 0m;
            decimal outflow = 0m;

            foreach (var tx in transactions)
            {
                var transactionValue = 0m;
                if (tx.Currency != account.Currency)
                {
                    transactionValue = await _currencyConversionClient.ConvertTransactionValueAsync(account.Currency, tx.Currency, tx.Value);
                }
                else
                {
                    transactionValue = tx.Value;
                }

                if (transactionValue > 0)
                {
                    inflow += transactionValue;
                }
                
                else if (transactionValue < 0)
                {
                    outflow += Math.Abs(transactionValue); 
                }
                
            }

            var netChange = inflow - outflow;
            var startBalance = account.Balance - netChange;

            var kpiResponse = new AccountKpiResponse(startBalance, inflow, outflow, netChange, startBalance + netChange);

            return Result<AccountKpiResponse>.Success(kpiResponse);
        }

        public async Task<Result<AccountBalanceTrendResponse>> GetAccountBalanceTrendAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, accountId.ToString());

            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);
            var account = await _accountsClient.GetAccountByIdAsync(accountId, token);

            var sortedTransactions = transactions.OrderBy(tx => tx.TransactionDate)
                                                 .ToList();

            var dailyBalances = new Dictionary<DateTime, decimal>();
            var startBalance = account.Balance;
            var convertedValues = new List<decimal>();
            foreach (var tx in sortedTransactions)
            {
                var transactionValue = 0m;
                if (tx.Currency != account.Currency)
                {
                    transactionValue = await _currencyConversionClient.ConvertTransactionValueAsync(account.Currency, tx.Currency, tx.Value);
                }
                else
                {
                    transactionValue = tx.Value;
                }
                startBalance -= transactionValue;
                convertedValues.Add(transactionValue);
            }
            decimal currentBalance = startBalance; 

            for(int i=0; i<sortedTransactions.Count; i++)
            {
                currentBalance += convertedValues[i];

                dailyBalances[sortedTransactions[i].TransactionDate.Date] = currentBalance;
            }

            var labels = new List<string>();
            var values = new List<decimal>();
            var lastKnownBalance = startBalance; 

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (dailyBalances.TryGetValue(date, out var balanceForDay))
                {
                    lastKnownBalance = balanceForDay;
                }

                labels.Add(date.ToString("yyyy-MM-dd"));
                values.Add(lastKnownBalance);
            }

            return Result<AccountBalanceTrendResponse>.Success(new AccountBalanceTrendResponse(labels, values));
        }

        public async Task<Result<AccountCategoryBreakdownResponse>> GetAccountCategoryBreakdownAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string type, CancellationToken token)
        {
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, type.ToString(), null, null, accountId.ToString());

            var allTransactionsResult = await _transactionsClient.GetTransactionsAsync(filter, token);
            var account = await _accountsClient.GetAccountByIdAsync(accountId, token);

            IEnumerable<TransactionResponse> filteredTransactions;
            bool isIncome = type.Equals("INCOME", StringComparison.OrdinalIgnoreCase); 

            if (isIncome)
            {
                filteredTransactions = allTransactionsResult
                    .Where(tx => tx.TransactionType.Equals("INCOME", StringComparison.OrdinalIgnoreCase));
            }
            else 
            {
                filteredTransactions = allTransactionsResult
                   .Where(tx => tx.TransactionType.Equals("EXPENSE", StringComparison.OrdinalIgnoreCase));
            }

            foreach(var tx in filteredTransactions)
            {
                if (tx.Currency != account.Currency)
                {
                    tx.Value = await _currencyConversionClient.ConvertTransactionValueAsync(account.Currency, tx.Currency, tx.Value);
                    tx.Currency = account.Currency;
                }
            }
            var groupedSums = filteredTransactions
                .GroupBy(tx => tx.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    TotalValue = g.Sum(tx => Math.Abs(tx.Value))
                })
                .Where(g => g.TotalValue > 0)
                .ToList();

            if (!groupedSums.Any())
            {
                return Result<AccountCategoryBreakdownResponse>.Success(new AccountCategoryBreakdownResponse(new List<long>(), new List<decimal>()));
            }

            var labels = new List<long>();
            var values = new List<decimal>();
            foreach (var group in groupedSums.OrderByDescending(g => g.TotalValue)) 
            {
                labels.Add(group.CategoryId);
                values.Add(group.TotalValue);
                
            }

            return Result<AccountCategoryBreakdownResponse>.Success(new AccountCategoryBreakdownResponse(labels, values));
        }
    }
}
