using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnalyticsService.Messaging.DTO;
using AnalyticsService.Messaging.Http.Interfaces;
using AnalyticsService.Models.DTO.Response.SpendingPatternsReport;
using AnalyticsService.Models.Errors;
using AnalyticsService.Services.Interfaces;

namespace AnalyticsService.Services.Implementations
{
    public class SpendingPatternsReportService : ISpendingPatternsReportService
    {
        private readonly ITransactionsClient _transactionsClient;
        private readonly ICurrencyConversionClient _currencyConversionClient;

        public SpendingPatternsReportService(ITransactionsClient transactionsClient, ICurrencyConversionClient currencyConversionClient)
        {
            _transactionsClient = transactionsClient;
            _currencyConversionClient = currencyConversionClient;
        }

        public async Task<Result<SpendingPatternsKpiResponse>> GetSpendingPatternsKpiAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            // Получаем транзакции
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            // Конвертация и отбор расходов
            var expenses = new List<decimal>();
            foreach (var tx in transactions)
            {
                var value = tx.Currency != targetCurrency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value)
                    : tx.Value;

                if (value < 0)
                    expenses.Add(Math.Abs(value));
            }

            if (!expenses.Any())
            {
                return Result<SpendingPatternsKpiResponse>.Success(new SpendingPatternsKpiResponse());
            }

            var total = expenses.Sum();
            var days = (endDate.Date - startDate.Date).Days + 1;
            var avgDaily = total / days;
            var avgCheck = expenses.Average();

            // Самый затратный день
            var groupByDay = transactions
                .Where(tx => tx.Value < 0)
                .GroupBy(tx => tx.TransactionDate.Date)
                .Select(g => new { Date = g.Key, Sum = g.Sum(t => Math.Abs(t.Value)) });
            var busiest = groupByDay.OrderByDescending(g => g.Sum).First().Date.ToString("yyyy-MM-dd");

            var dto = new SpendingPatternsKpiResponse
            {
                TotalSpending = total,
                AvgDailySpending = avgDaily,
                TransactionCount = expenses.Count,
                AvgCheck = avgCheck,
                BusiestDay = busiest
            };
            return Result<SpendingPatternsKpiResponse>.Success(dto);
        }

        public async Task<Result<List<DayOfWeekPatternResponse>>> GetDayOfWeekPatternsAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var ru = new CultureInfo("ru-RU");
            var result = transactions
                .Where(tx => tx.Value < 0)
                .GroupBy(tx => tx.TransactionDate.DayOfWeek)
                .Select(g =>
                {
                    var absVals = g.Select(t => Math.Abs(t.Value));
                    return new DayOfWeekPatternResponse
                    {
                        DayIndex = (int)g.Key,
                        DayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ru.DateTimeFormat.GetDayName(g.Key)),
                        TotalAmount = absVals.Sum(),
                        AvgAmount = absVals.Any() ? absVals.Average() : 0,
                        Count = absVals.Count()
                    };
                })
                .OrderBy(d => d.DayIndex)
                .ToList();

            return Result<List<DayOfWeekPatternResponse>>.Success(result);
        }

        public async Task<Result<List<TimeOfDayPatternResponse>>> GetTimeOfDayPatternsAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            // Определяем периоды
            var buckets = new[]
            {
                (Name: "Ночь", From: 0, To: 6),
                (Name: "Утро", From: 6, To: 12),
                (Name: "День", From: 12, To: 18),
                (Name: "Вечер", From: 18, To: 24)
            };

            var list = new List<TimeOfDayPatternResponse>();
            foreach (var b in buckets)
            {
                var sum = transactions
                    .Where(tx => tx.Value < 0 && tx.TransactionDate.Hour >= b.From && tx.TransactionDate.Hour < b.To)
                    .Select(tx => tx.Currency != targetCurrency
                        ? _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value).Result
                        : tx.Value)
                    .Select(Math.Abs)
                    .Sum();

                list.Add(new TimeOfDayPatternResponse { PeriodName = b.Name, TotalAmount = sum });
            }
            return Result<List<TimeOfDayPatternResponse>>.Success(list);
        }

        public async Task<Result<ValueDistributionResponse>> GetValueDistributionAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var values = transactions.Where(tx => tx.Value < 0)
                .Select(tx => tx.Currency != targetCurrency
                    ? _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value).Result
                    : tx.Value)
                .Select(Math.Abs)
                .ToList();

            if (!values.Any())
                return Result<ValueDistributionResponse>.Success(new ValueDistributionResponse());

            var max = values.Max();
            var binCount = 5;
            var width = max / binCount;
            var bins = new List<ValueDistributionBinDto>();
            for (int i = 0; i < binCount; i++)
            {
                var lower = i * width;
                var upper = i == binCount - 1 ? max : (i + 1) * width;
                var count = values.Count(v => v >= lower && v < upper);
                bins.Add(new ValueDistributionBinDto
                {
                    Label = i == binCount - 1
                        ? $"{lower:0.##}+"
                        : $"{lower:0.##}-{upper:0.##}",
                    Count = count
                });
            }
            return Result<ValueDistributionResponse>.Success(new ValueDistributionResponse { Bins = bins });
        }

        public async Task<Result<AvgCheckTrendResponse>> GetAvgCheckTrendAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var byDate = transactions.Where(tx => tx.Value < 0)
                .GroupBy(tx => tx.TransactionDate.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(tx => tx.Currency != targetCurrency
                            ? _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value).Result
                            : tx.Value)
                        .Select(Math.Abs).
                        Average()
                );

            var labels = new List<string>();
            var values = new List<decimal>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                labels.Add(date.ToString("yyyy-MM-dd"));
                values.Add(byDate.ContainsKey(date) ? byDate[date] : 0);
            }

            return Result<AvgCheckTrendResponse>.Success(new AvgCheckTrendResponse { Labels = labels, Values = values });
        }

        public async Task<Result<List<LargestTransactionResponse>>> GetLargestTransactionsAsync(
            DateTime startDate,
            DateTime endDate,
            string targetCurrency,
            string? accountIds,
            string? categoryIds,
            CancellationToken token)
        {
            var filter = new TransactionFilterParameters(
                1, /*Вставить нужный userId*/1, null, startDate, endDate, categoryIds, "EXPENSE", null, null, accountIds);
            var transactions = await _transactionsClient.GetTransactionsAsync(filter, token);

            var converted = new List<(TransactionResponse Tx, decimal Value)>();
            foreach (var tx in transactions)
            {
                var val = tx.Currency != targetCurrency
                    ? await _currencyConversionClient.ConvertTransactionValueAsync(targetCurrency, tx.Currency, tx.Value)
                    : tx.Value;
                converted.Add((tx, Math.Abs(val)));
            }

            var top = converted.OrderByDescending(x => x.Value).Take(5);
            var list = top.Select(x => new LargestTransactionResponse
            {
                Id = x.Tx.Id,
                TransactionDate = x.Tx.TransactionDate,
                Title = x.Tx.Description,
                CategoryId = x.Tx.CategoryId,
                CategoryName = null,
                Amount = x.Value
            }).ToList();

            return Result<List<LargestTransactionResponse>>.Success(list);
        }
    }
}
