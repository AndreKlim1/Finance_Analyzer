using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Models.DTO.Response.BudgetAnalytics;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations
{
    public class BudgetAnalyticsService : IBudgetAnalyticsService
    {
        private readonly ITransactionsClient _transactionsClient;
        private readonly IBudgetsClient _budgetsClient;
        private readonly ICurrencyConversionClient _currencyConversionClient;

        public BudgetAnalyticsService(ITransactionsClient transactionsClient, IBudgetsClient budgetsClient, ICurrencyConversionClient currencyConversionClient)
        {
            _transactionsClient = transactionsClient;
            _budgetsClient = budgetsClient;
            _currencyConversionClient = currencyConversionClient;
        }

        public async Task<Result<BudgetCategoryBreakdownResponse>> GetBudgetCategoryBreakdownAsync(long userId, long budgetId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            //!!!!!!!!!!!!!!!!!!!
            var budget = await _budgetsClient.GetBudgetByIdAsync(budgetId, token);
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, null);

            var allTransactionsResult = await _transactionsClient.GetTransactionsAsync(filter, token);
            

            IEnumerable<TransactionResponse> filteredTransactions;
            bool isIncome = budget.BudgetType.Equals("SAVINGS", StringComparison.OrdinalIgnoreCase);

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

            foreach (var tx in filteredTransactions)
            {
                if (tx.Currency != budget.Currency)
                {
                    tx.Value = await _currencyConversionClient.ConvertTransactionValueAsync(budget.Currency, tx.Currency, tx.Value);
                    tx.Currency = budget.Currency;
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
                return Result<BudgetCategoryBreakdownResponse>.Success(new BudgetCategoryBreakdownResponse(new List<long>(), new List<decimal>()));
            }

            var labels = new List<long>();
            var values = new List<decimal>();
            foreach (var group in groupedSums.OrderByDescending(g => g.TotalValue))
            {
                labels.Add(group.CategoryId);
                values.Add(group.TotalValue);

            }

            return Result<BudgetCategoryBreakdownResponse>.Success(new BudgetCategoryBreakdownResponse(labels, values));
        }

        public async Task<Result<BudgetTrendResponse>> GetBudgetTrendAsync(long userId, long budgetId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            //!!!!!!!!!
            var budget = await _budgetsClient.GetBudgetByIdAsync(budgetId, token);
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, null);

            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var sortedTransactions = transactions.OrderBy(tx => tx.TransactionDate)
                                                 .ToList();

            var dailyBalances = new Dictionary<DateTime, decimal>();
            var startBalance = 0m;
            var convertedValues = new List<decimal>();
            foreach (var tx in sortedTransactions)
            {
                var transactionValue = 0m;
                if (tx.Currency != budget.Currency)
                {
                    transactionValue = await _currencyConversionClient.ConvertTransactionValueAsync(budget.Currency, tx.Currency, tx.Value);
                }
                else
                {
                    transactionValue = tx.Value;
                }
                startBalance -= transactionValue;
                convertedValues.Add(transactionValue);
            }
            decimal currentBalance = startBalance;

            for (int i = 0; i < sortedTransactions.Count; i++)
            {
                currentBalance += convertedValues[i];

                dailyBalances[sortedTransactions[i].TransactionDate.Date] = currentBalance;
            }

            var labels = new List<string>();
            var values = new List<decimal>();
            var lastKnownBalance = startBalance;

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                labels.Add(date.ToString("yyyy-MM-dd"));
                values.Add(dailyBalances.TryGetValue(date, out var balanceForDay) ? balanceForDay : 0);
            }

            return Result<BudgetTrendResponse>.Success(new BudgetTrendResponse(labels, values));
        }
    }
}
