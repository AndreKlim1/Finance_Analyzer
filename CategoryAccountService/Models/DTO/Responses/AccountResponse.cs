﻿
namespace CaregoryAccountService.Models.DTO.Responses
{
    public record AccountResponse(
        long Id,
        long UserId,
        string AccountName,
        string AccountType,
        string Currency,
        decimal Balance,
        int TransactionsCount,
        string Description,
        string Color);
}
