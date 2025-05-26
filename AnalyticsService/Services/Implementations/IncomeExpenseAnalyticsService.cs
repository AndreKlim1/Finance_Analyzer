using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Models.DTO.Response.IncomeExpenseReport;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations
{
    public class IncomeExpenseAnalyticsService : IIncomeExpenseAnalyticsService
    {
        private readonly ITransactionsClient _transactionsClient;
        private readonly ICurrencyConversionClient _currencyConversionClient;

        public IncomeExpenseAnalyticsService(
            ITransactionsClient transactionsClient,
            ICurrencyConversionClient currencyConversionClient)
        {
            _transactionsClient = transactionsClient;
            _currencyConversionClient = currencyConversionClient;
        }

        public async Task<Result<ComparisonResponse<IncomeExpenseKpiDto>>> GetKpiAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD")
        {
            var primary = await ComputeKpiAsync(primaryStartDate, primaryEndDate, accountIds, categoryIds, token, currency, userId);
            var comparison = await ComputeKpiAsync(compareStartDate, compareEndDate, accountIds, categoryIds, token, currency, userId);

            var result = new ComparisonResponse<IncomeExpenseKpiDto>
            {
                PrimaryData = primary.Value,
                ComparisonData = comparison.Value
            };
            return Result<ComparisonResponse<IncomeExpenseKpiDto>>.Success(result);
        }

        private async Task<Result<IncomeExpenseKpiDto>> ComputeKpiAsync(
            DateTime start,
            DateTime end,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var filter = new TransactionFilterParameters(
                1, userId,
                null, start, end,
                categoryIds, null, null, null,
                accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            decimal income = 0m, expense = 0m;
            foreach (var tx in transactions)
            {
                var value = tx.Currency != currency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(tx.Currency, tx.Currency, tx.Value)
                    : tx.Value;
                if (value >= 0) income += value;
                else expense += Math.Abs(value);
            }

            return Result<IncomeExpenseKpiDto>.Success(new IncomeExpenseKpiDto
            {
                Income = income,
                Expense = expense
            });
        }

        public async Task<Result<ComparisonResponse<IncomeExpenseTrendResponse>>> GetTrendAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD")
        {
            var primary = await ComputeTrendAsync(primaryStartDate, primaryEndDate, groupBy, accountIds, categoryIds, token, currency, userId);
            var comparison = await ComputeTrendAsync(compareStartDate, compareEndDate, groupBy, accountIds, categoryIds, token, currency, userId);

            var result = new ComparisonResponse<IncomeExpenseTrendResponse>
            {
                PrimaryData = primary.Value,
                ComparisonData = comparison.Value
            };
            return Result<ComparisonResponse<IncomeExpenseTrendResponse>>.Success(result);
        }

        private async Task<Result<IncomeExpenseTrendResponse>> ComputeTrendAsync(
            DateTime start,
            DateTime end,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var filter = new TransactionFilterParameters(
                1, userId, null, start, end,
                categoryIds, null, null, null,
                accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var labels = new List<string>();
            var incomeSeries = new List<decimal>();
            var expenseSeries = new List<decimal>();
            var cursor = start;

            while (cursor.Date <= end.Date)
            {
                var periodKey = KeyOf(cursor, groupBy);
                var slice = transactions.Where(tx => KeyOf(tx.TransactionDate, groupBy) == periodKey);

                decimal inc = 0m, exp = 0m;
                foreach (var tx in slice)
                {
                    var val = tx.Currency != currency
                        ? await _currencyConversionClient.ConvertTransactionValueAsync(tx.Currency, tx.Currency, tx.Value)
                        : tx.Value;
                    if (val >= 0) inc += val;
                    else exp += Math.Abs(val);
                }

                labels.Add(periodKey);
                incomeSeries.Add(inc);
                expenseSeries.Add(exp);

                cursor = AdvanceCursor(cursor, groupBy);
            }

            return Result<IncomeExpenseTrendResponse>.Success(new IncomeExpenseTrendResponse
            {
                Labels = labels,
                IncomeSeries = incomeSeries,
                ExpenseSeries = expenseSeries
            });
        }

        public async Task<Result<ComparisonResponse<IncomeExpenseBreakdownResponse>>> GetBreakdownAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD")
        {
            var primary = await ComputeBreakdownAsync(primaryStartDate, primaryEndDate, accountIds, categoryIds, token, currency, userId);
            var comparison = await ComputeBreakdownAsync(compareStartDate, compareEndDate, accountIds, categoryIds, token, currency, userId);

            var result = new ComparisonResponse<IncomeExpenseBreakdownResponse>
            {
                PrimaryData = primary.Value,
                ComparisonData = comparison.Value
            };
            return Result<ComparisonResponse<IncomeExpenseBreakdownResponse>>.Success(result);
        }

        private async Task<Result<IncomeExpenseBreakdownResponse>> ComputeBreakdownAsync(
            DateTime start,
            DateTime end,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var filter = new TransactionFilterParameters(
                1, userId, null, start, end,
                categoryIds, null, null, null,
                accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);
            for(int i=0; i<transactions.Count; i++)
            {
                var value = transactions[i].Currency != currency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(transactions[i].Currency, transactions[i].Currency, transactions[i].Value)
                    : transactions[i].Value;
                transactions[i].Value = value;
            }

            var incomeGroup = transactions.Where(tx => tx.Value >= 0)
                .GroupBy(tx => tx.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(tx => tx.Value));
            var expenseGroup = transactions.Where(tx => tx.Value < 0)
                .GroupBy(tx => tx.CategoryId)
                .ToDictionary(g => g.Key, g => g.Sum(tx => Math.Abs(tx.Value)));

            var breakdown = new IncomeExpenseBreakdownResponse
            {
                Income = new BreakdownDto
                {
                    Labels = incomeGroup.Keys.Select(k => k.ToString()).ToList(),
                    Values = incomeGroup.Values.ToList()
                },
                Expense = new BreakdownDto
                {
                    Labels = expenseGroup.Keys.Select(k => k.ToString()).ToList(),
                    Values = expenseGroup.Values.ToList()
                }
            };
            return Result<IncomeExpenseBreakdownResponse>.Success(breakdown);
        }

        public async Task<Result<ComparisonResponse<IEnumerable<TimeTableRowResponse>>>> GetTimeTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD")
        {
            var primary = await ComputeTimeTableAsync(primaryStartDate, primaryEndDate, groupBy, accountIds, categoryIds, token, currency, userId);
            var comparison = await ComputeTimeTableAsync(compareStartDate, compareEndDate, groupBy, accountIds, categoryIds, token, currency, userId);

            var result = new ComparisonResponse<IEnumerable<TimeTableRowResponse>>
            {
                PrimaryData = primary.Value,
                ComparisonData = comparison.Value
            };
            return Result<ComparisonResponse<IEnumerable<TimeTableRowResponse>>>.Success(result);
        }

        private async Task<Result<IEnumerable<TimeTableRowResponse>>> ComputeTimeTableAsync(
            DateTime start,
            DateTime end,
            string groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var filter = new TransactionFilterParameters(
                1, userId, null, start, end,
                categoryIds, null, null, null,
                accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);
            for (int i = 0; i < transactions.Count; i++)
            {
                var value = transactions[i].Currency != currency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(transactions[i].Currency, transactions[i].Currency, transactions[i].Value)
                    : transactions[i].Value;
                transactions[i].Value = value;
            }

            var rows = new List<TimeTableRowResponse>();
            var cursor = start;
            while (cursor.Date <= end.Date)
            {
                var period = KeyOf(cursor, groupBy);
                var slice = transactions.Where(tx => KeyOf(tx.TransactionDate, groupBy) == period);
                var inc = slice.Where(tx => tx.Value >= 0).Sum(tx => tx.Value);
                var exp = slice.Where(tx => tx.Value < 0).Sum(tx => Math.Abs(tx.Value));
                rows.Add(new TimeTableRowResponse { Period = period, Income = inc, Expense = exp });
                cursor = AdvanceCursor(cursor, groupBy);
            }
            return Result<IEnumerable<TimeTableRowResponse>>.Success(rows);
        }

        public async Task<Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetSourceTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD"
            )
        {
            return await GetCategoryAmountComparisonAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                true, accountIds, categoryIds, token, currency, userId);
        }

        public async Task<Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetCategoryTableAsync(
            DateTime primaryStartDate,
            DateTime primaryEndDate,
            DateTime compareStartDate,
            DateTime compareEndDate,
            string? groupBy,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            long userId,
            string currency = "USD")
        {
            return await GetCategoryAmountComparisonAsync(
                primaryStartDate, primaryEndDate,
                compareStartDate, compareEndDate,
                false, accountIds, categoryIds, token, currency, userId);
        }

        private async Task<Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>> GetCategoryAmountComparisonAsync(
            DateTime ps,
            DateTime pe,
            DateTime cs,
            DateTime ce,
            bool income,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var primary = await ComputeCategoryAmountsAsync(ps, pe, income, accountIds, categoryIds, token, currency, userId);
            var comparison = await ComputeCategoryAmountsAsync(cs, ce, income, accountIds, categoryIds, token, currency, userId);
            var result = new ComparisonResponse<IEnumerable<CategoryAmountDto>>
            {
                PrimaryData = primary.Value,
                ComparisonData = comparison.Value
            };
            return Result<ComparisonResponse<IEnumerable<CategoryAmountDto>>>.Success(result);
        }

        private async Task<Result<IEnumerable<CategoryAmountDto>>> ComputeCategoryAmountsAsync(
            DateTime start,
            DateTime end,
            bool income,
            string? accountIds,
            string? categoryIds,
            CancellationToken token,
            string currency,
            long userId)
        {
            var filter = new TransactionFilterParameters(
                1, userId, null, start, end,
                categoryIds, null, null, null,
                accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);
            for (int i = 0; i < transactions.Count; i++)
            {
                var value = transactions[i].Currency != currency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(transactions[i].Currency, transactions[i].Currency, transactions[i].Value)
                    : transactions[i].Value;
                transactions[i].Value = value;
            }

            var grouped = income
                ? transactions.Where(tx => tx.Value >= 0).GroupBy(tx => tx.CategoryId)
                : transactions.Where(tx => tx.Value < 0).GroupBy(tx => tx.CategoryId);

            var list = grouped
                .Select(g => new CategoryAmountDto
                {
                    Id = g.Key,
                    Amount = income ? g.Sum(tx => tx.Value) : g.Sum(tx => Math.Abs(tx.Value))
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            return Result<IEnumerable<CategoryAmountDto>>.Success(list);
        }

        private static string KeyOf(DateTime dt, string groupBy) => groupBy.ToLower() switch
        {
            "month" => dt.ToString("yyyy-MM"),
            "year" => dt.ToString("yyyy"),
            _ => dt.ToString("yyyy-MM-dd")
        };

        private static DateTime AdvanceCursor(DateTime cursor, string groupBy) => groupBy.ToLower() switch
        {
            "month" => cursor.AddMonths(1),
            "year" => cursor.AddYears(1),
            _ => cursor.AddDays(1)
        };
    }
}