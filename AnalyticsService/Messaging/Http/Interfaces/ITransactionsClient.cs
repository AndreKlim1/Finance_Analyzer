﻿using AnalyticsService.Messaging.DTO;

namespace AnalyticsService.Messaging.Http.Interfaces
{
    public interface ITransactionsClient
    {
        Task<List<TransactionResponse>> GetTransactionsAsync(TransactionFilterParameters filter, CancellationToken cancellationToken);
    }
}
