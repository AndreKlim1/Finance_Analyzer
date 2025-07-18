﻿using System.Runtime.CompilerServices;
using TransactionsService.Models;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.Enums;


namespace TransactionsService.Services.Mappings
{
    public static class TransactionMapping
    {
        public static TransactionResponse ToTransactionResponse(this Transaction transaction)
        {
            return new TransactionResponse
            (
                transaction.Id,
                transaction.Value,
                transaction.Title,
                transaction.Currency.ToString(),
                transaction.CategoryId,
                transaction.AccountId,
                transaction.UserId,
                transaction.Description,
                transaction.Image,
                transaction.TransactionDate,
                transaction.CreationDate,
                transaction.TransactionType.ToString(),
                transaction.Merchant
            );
        }

        public static Transaction ToTransaction(this CreateTransactionRequest request)
        {
            return new Transaction
            {
                Value = request.Value,
                Title = request.Title,
                Currency = Enum.Parse<Currency>(request.Currency),
                CategoryId = request.CategoryId,
                AccountId = request.AccountId,
                UserId = request.UserId,
                Description = request.Description,
                Image = request.Image,
                TransactionDate = request.TransactionDate,
                CreationDate = request.CreationDate,
                TransactionType = Enum.Parse<TransactionType>(request.TransactionType),
                Merchant = request.Merchant
            };
        }

        public static Transaction ToTransaction(this UpdateTransactionRequest request)
        {
            return new Transaction
            {
                Id = request.Id,
                Value = request.Value,
                Title = request.Title,
                Currency = Enum.Parse<Currency>(request.Currency),
                CategoryId = request.CategoryId,
                AccountId = request.AccountId,
                UserId = request.UserId,
                Description = request.Description,
                Image = request.Image,
                TransactionDate = request.TransactionDate,
                CreationDate = request.CreationDate,
                TransactionType = Enum.Parse<TransactionType>(request.TransactionType),
                Merchant = request.Merchant
            };
        }
    }
}
