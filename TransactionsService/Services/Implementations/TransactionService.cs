using FluentValidation;
using TransactionsService.Services.Interfaces;
using TransactionsService.Repositories.Interfaces;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.Errors;
using TransactionsService.Services.Mappings;
using TransactionsService.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using TransactionsService.Messaging;
using TransactionsService.Messaging.Events;
using TransactionsService.Models;

namespace TransactionsService.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IKafkaProducer _kafkaProducer;
        

        public TransactionService(ITransactionRepository transactionRepository, IKafkaProducer kafkaProducer)
        {
            _transactionRepository = transactionRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<Result<TransactionResponse>> GetTransactionByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, token);

            return transaction is null
                ? Result<TransactionResponse>.Failure(TransactionErrors.TransactionNotFound)
                : Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest,
            CancellationToken token)
        {

            var transaction = createTransactionRequest.ToTransaction();

            await _transactionRepository.AddAsync(transaction, token);

            var transactionCreatedEvent = new TransactionEvent("transaction-created", new TransactionData(transaction));

            await _kafkaProducer.ProduceAsync("transaction-events", transaction.Id.ToString(), transactionCreatedEvent);

            return Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<TransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest,
            CancellationToken token)
        {
            var transaction = updateTransactionRequest.ToTransaction();
            var prevTransaction = await _transactionRepository.GetByIdAsync(transaction.Id, token);
            var prevTransactionData = new TransactionData(prevTransaction);
            transaction = await _transactionRepository.UpdateAsync(transaction, token);
            if(transaction != null)
            {
                var transactionUpdatedEvent = new TransactionUpdateEvent(prevTransactionData, new TransactionData(transaction));
                await _kafkaProducer.ProduceAsync("transaction-update-events", transaction.Id.ToString(), transactionUpdatedEvent);
                return Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
            }

            return Result<TransactionResponse>.Failure(TransactionErrors.TransactionNotFound);
                
        }

        public async Task<bool> DeleteTransactionAsync(long id, CancellationToken token)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, token);
            if (transaction is not null)
            {
                var transactionDeletedEvent = new TransactionEvent("transaction-deleted", new TransactionData(transaction));
                transactionDeletedEvent.Data.Value *= -1;
                await _kafkaProducer.ProduceAsync("transaction-events", transaction.Id.ToString(), transactionDeletedEvent);
                await _transactionRepository.DeleteAsync(id, token);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Result<List<TransactionResponse>>> GetTransactionsAsync(CancellationToken token)
        {
            var transactions = await _transactionRepository.FindAll(true).ToListAsync();
            if (transactions is null)
            {
                return Result<List<TransactionResponse>>.Failure(TransactionErrors.TransactionNotFound);
            }
            else
            {
                var responses = new List<TransactionResponse>();
                foreach (var transaction in transactions)
                {
                    responses.Add(transaction.ToTransactionResponse());
                }
                return Result<List<TransactionResponse>>.Success(responses);
            }
        }

        public async Task<Result<List<TransactionResponse>>> GetTransactionsByUserIdAsync(long userId, CancellationToken token)
        {
            var transactions = await _transactionRepository.FindByCondition(l => l.UserId == userId, true).ToListAsync();
            if (transactions is null)
            {
                return Result<List<TransactionResponse>>.Failure(TransactionErrors.TransactionNotFound);
            }
            else
            {
                var responses = new List<TransactionResponse>();
                foreach (var transaction in transactions)
                {
                    responses.Add(transaction.ToTransactionResponse());
                }
                return Result<List<TransactionResponse>>.Success(responses);
            }
        }

        public async Task<bool> DeleteTransactionsByAccountIdAsync(long accountId, CancellationToken token)
        {
            var transactions = await _transactionRepository.FindByCondition(l => l.AccountId == accountId, true).ToListAsync();
            if(transactions is not null)
            {
                foreach (var transaction in transactions)
                {
                    await _transactionRepository.DeleteAsync(transaction.Id, token);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteTransactionsByCategoryIdAsync(long categoryId, CancellationToken token)
        {
            var transactions = await _transactionRepository.FindByCondition(l => l.CategoryId == categoryId, true).ToListAsync();
            if (transactions is not null)
            {
                foreach (var transaction in transactions)
                {
                    await _transactionRepository.DeleteAsync(transaction.Id, token);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Result<TransactionResponse>> CreateTransactionFromAccountAsync(CreateTransactionRequest createTransactionRequest, CancellationToken token)
        {
            var transaction = createTransactionRequest.ToTransaction();
            await _transactionRepository.AddAsync(transaction, token);

            return Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }
    }
}
