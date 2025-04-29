using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Models.DTO.Response.AnalyticsDashboard;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations
{
    public class AnalyticsDashboardService : IAnalyticsDashboardService
    {
        private readonly ITransactionsClient _transactionsClient;
        private readonly IAccountsClient _accountsClient;
        private readonly ICurrencyConversionClient _currencyConversionClient;

        public AnalyticsDashboardService(
            ITransactionsClient transactionsClient,
            IAccountsClient accountsClient,
            ICurrencyConversionClient currencyConversionClient)
        {
            _transactionsClient = transactionsClient;
            _accountsClient = accountsClient;
            _currencyConversionClient = currencyConversionClient;
        }

        public async Task<Result<KpiResponse>> GetKpiAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            var userAccount = await _accountsClient.GetAccountByIdAsync(accountId, token);
            if (userAccount == null)
            {
                return Result<KpiResponse>.Success(new KpiResponse(0m, 0m, 0m));
            }
            
            string targetCurrency = userAccount.Currency;
            
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, null);
            var transactions = await  _transactionsClient.GetTransactionsAsync(filter, token);

            decimal totalIncome = 0m;
            decimal totalExpense = 0m;

            foreach (var tx in transactions)
            {
                decimal convertedValue = tx.Value;
                
                if (tx.Currency != targetCurrency)
                {
                    convertedValue = await _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value);
                }

                if (convertedValue > 0)
                {
                    totalIncome += convertedValue;
                }
                else if (convertedValue < 0)
                {
                    totalExpense += Math.Abs(convertedValue);
                }
            }

            var netFlow = totalIncome - totalExpense;
            var kpiResponse = new KpiResponse(totalIncome, totalExpense, netFlow);

            return Result<KpiResponse>.Success(kpiResponse);
        }

        public async Task<Result<IncomeExpenseTrendResponse>> GetIncomeExpenseTrendAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string granularity, CancellationToken token)
        {
            var userAccount = await _accountsClient.GetAccountByIdAsync(accountId, token);
            if (userAccount == null)
            {
                return Result<IncomeExpenseTrendResponse>.Success(new IncomeExpenseTrendResponse(new List<string>(), new List<decimal>(), new List<decimal>()));
            }

            string targetCurrency = userAccount.Currency;

            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, null);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var convertedTransactions = new List<(DateTime Date, decimal Income, decimal Expense)>();
            foreach (var tx in transactions)
            {
                decimal convertedValue = tx.Value;
                if (tx.Currency != targetCurrency)
                {
                    convertedValue = await _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value);
                }

                decimal income = 0m;
                decimal expense = 0m;
                if (convertedValue > 0) income = convertedValue;
                else if (convertedValue < 0) expense = Math.Abs(convertedValue);

                convertedTransactions.Add((tx.TransactionDate, income, expense));
            }

            // --- Group by Granularity ---
            Func<DateTime, DateTime> groupByFunc;
            string labelFormat;

            switch (granularity?.ToLowerInvariant())
            {
                case "daily":
                default: // Default to daily
                    groupByFunc = dt => dt.Date;
                    labelFormat = "yyyy-MM-dd";
                    break;
                case "weekly":
                    // Adjust for start of week (e.g., Monday)
                    groupByFunc = dt => dt.Date.AddDays(-(int)(dt.DayOfWeek == DayOfWeek.Sunday ? 6 : dt.DayOfWeek - DayOfWeek.Monday));
                    labelFormat = "yyyy-MM-dd"; // Label is start of week
                    break;
                case "monthly":
                    groupByFunc = dt => new DateTime(dt.Year, dt.Month, 1);
                    labelFormat = "yyyy-MM";
                    break;
            }

            var groupedData = convertedTransactions
                .GroupBy(t => groupByFunc(t.Date))
                .Select(g => new
                {
                    PeriodStart = g.Key,
                    TotalIncome = g.Sum(t => t.Income),
                    TotalExpense = g.Sum(t => t.Expense)
                })
                .OrderBy(g => g.PeriodStart)
                .ToDictionary(g => g.PeriodStart);

            var labels = new List<string>();
            var incomeSeries = new List<decimal>();
            var expenseSeries = new List<decimal>();

            // Determine the loop increment based on granularity
            TimeSpan increment;
            DateTime loopEndDate = endDate.Date;
            DateTime currentPeriodStart = groupByFunc(startDate.Date); // Align start date to period start

            switch (granularity?.ToLowerInvariant())
            {
                case "weekly":
                    increment = TimeSpan.FromDays(7);
                    break;
                case "monthly":
                    // Monthly increment is tricky, loop differently
                    increment = TimeSpan.Zero; // Will handle in loop
                    break;
                case "daily":
                default:
                    increment = TimeSpan.FromDays(1);
                    break;
            }

            DateTime loopDate = currentPeriodStart;

            while (loopDate <= loopEndDate)
            {
                DateTime periodKey = loopDate;
                if (granularity?.ToLowerInvariant() == "monthly")
                {
                    periodKey = new DateTime(loopDate.Year, loopDate.Month, 1);
                }


                labels.Add(periodKey.ToString(labelFormat));

                if (groupedData.TryGetValue(periodKey, out var data))
                {
                    incomeSeries.Add(data.TotalIncome);
                    expenseSeries.Add(data.TotalExpense);
                }
                else
                {
                    incomeSeries.Add(0m);
                    expenseSeries.Add(0m);
                }

                // Increment loopDate
                if (granularity?.ToLowerInvariant() == "monthly")
                {
                    // Ensure we don't miss the last month if endDate is mid-month
                    if (loopDate.Year == endDate.Year && loopDate.Month == endDate.Month) break; // Stop after processing the end month
                    loopDate = loopDate.AddMonths(1);
                    if (loopDate > endDate) loopDate = endDate; // Adjust if AddMonths skips over endDate
                }
                else
                {
                    loopDate = loopDate.Add(increment);
                }
            }


            var trendResponse = new IncomeExpenseTrendResponse(labels, incomeSeries, expenseSeries);
            return Result<IncomeExpenseTrendResponse>.Success(trendResponse);
        }

        public async Task<Result<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(long userId, long accountId, DateTime startDate, DateTime endDate, string type, CancellationToken token)
        {
            var userAccount = await _accountsClient.GetAccountByIdAsync(accountId, token);
            if (userAccount == null)
            {
                return Result<CategoryBreakdownResponse>.Success(new CategoryBreakdownResponse(new List<long>(), new List<decimal>()));
            }

            string targetCurrency = userAccount.Currency;

            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, null);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            // --- Filter by Type and Convert ---
            bool isIncome = type.Equals("income", StringComparison.OrdinalIgnoreCase);
            var relevantTransactions = new List<(long CategoryId, decimal ConvertedValue)>();

            foreach (var tx in transactions)
            {
                // Determine if transaction matches the requested type
                bool typeMatch = (isIncome && tx.Value > 0) || (!isIncome && tx.Value < 0);
                // Alternative using TransactionType field:
                // bool typeMatch = (isIncome && tx.TransactionType.Equals("INCOME", StringComparison.OrdinalIgnoreCase)) ||
                //                  (!isIncome && tx.TransactionType.Equals("EXPENSE", StringComparison.OrdinalIgnoreCase));

                if (typeMatch)
                {
                    decimal convertedValue = tx.Value;
                    if (tx.Currency != targetCurrency)
                    {
                        // TODO: Add error handling for conversion failure
                        convertedValue = await _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value);
                    }
                    relevantTransactions.Add((tx.CategoryId, convertedValue));
                }
            }

            // --- Group and Sum ---
            var groupedSums = relevantTransactions
                .GroupBy(t => t.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    TotalValue = g.Sum(t => Math.Abs(t.ConvertedValue)) // Sum absolute values for breakdown
                })
                .Where(g => g.TotalValue > 0) // Exclude zero sums
                .ToList();

            if (!groupedSums.Any())
            {
                return Result<CategoryBreakdownResponse>.Success(new CategoryBreakdownResponse(new List<long>(), new List<decimal>()));
            }

            var labels = new List<long>();
            var values = new List<decimal>();

            foreach (var group in groupedSums.OrderByDescending(g => g.TotalValue))
            {
                labels.Add(group.CategoryId);
                values.Add(group.TotalValue);
            }

            var breakdownResponse = new CategoryBreakdownResponse(labels, values);
            return Result<CategoryBreakdownResponse>.Success(breakdownResponse);
        }

        public async Task<Result<SparklineResponse>> GetAccountSparklineAsync(long userId, long accountId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            var account = await _accountsClient.GetAccountByIdAsync(accountId, token);
            if (account == null) 
            {
                return Result<SparklineResponse>.Success(new SparklineResponse(new List<string>(), new List<decimal>()));
            }
            var filter = new TransactionFilterParameters(1, userId, null, startDate, endDate, null, null, null, null, accountId.ToString());
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var sortedTransactions = transactions.OrderBy(tx => tx.TransactionDate).ToList();

            decimal netChangeInPeriod = 0m;
            foreach (var tx in sortedTransactions)
            {
                decimal transactionValue = tx.Value;
                if (tx.Currency != account.Currency)
                {
                    transactionValue = await _currencyConversionClient.ConvertTransactionValueAsync(account.Currency, tx.Currency, tx.Value);
                }
                netChangeInPeriod += transactionValue;
            }
            decimal startBalance = account.Balance - netChangeInPeriod;

            var dailyBalances = new Dictionary<DateTime, decimal>();
            decimal currentBalance = startBalance;

            var convertedValues = new List<(DateTime Date, decimal Value)>();
            foreach (var tx in sortedTransactions)
            {
                decimal transactionValue = tx.Value;
                if (tx.Currency != account.Currency)
                {
                    transactionValue = await _currencyConversionClient.ConvertTransactionValueAsync(account.Currency, tx.Currency, tx.Value);
                }
                convertedValues.Add((tx.TransactionDate, transactionValue));
            }

            var dailyChanges = convertedValues
                .GroupBy(cv => cv.Date.Date)
                .ToDictionary(g => g.Key, g => g.Sum(cv => cv.Value));

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (dailyChanges.TryGetValue(date, out var changeForDay))
                {
                    currentBalance += changeForDay;
                }
                dailyBalances[date] = currentBalance;
            }

            var labels = new List<string>();
            var values = new List<decimal>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                labels.Add(date.ToString("yyyy-MM-dd")); 
                values.Add(dailyBalances.TryGetValue(date, out var bal) ? bal : values.LastOrDefault(startBalance));
            }

            return Result<SparklineResponse>.Success(new SparklineResponse(labels, values));
        }
    }
}
