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
using System.Linq.Expressions;
using System.Threading;
using LinqKit;

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

        public async Task<Result<PagedResponse<TransactionResponse>>> GetTransactionsAsync(TransactionFilterParameters filter, CancellationToken token)
        {
            Expression<Func<Transaction, bool>> expression = t => true;

            //expression = expression.And(t => t.UserId == filter.UsertId);
            if (filter.StartDate.HasValue)
                expression = expression.And(t => t.TransactionDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                expression = expression.And(t => t.TransactionDate <= filter.EndDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim();
                expression = expression.And(t =>
                    t.Title.Contains(term) ||
                    t.Description.Contains(term) ||
                    t.Merchant.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(filter.Categories))
            {
                var cats = filter.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(long.Parse)
                                            .ToList();
                expression = expression.And(t => cats.Contains(t.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(filter.Types))
            {
                var types = filter.Types.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .ToList();
                expression = expression.And(t => types.Contains(t.TransactionType.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Accounts))
            {
                var accs = filter.Accounts.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(long.Parse)
                                          .ToList();
                expression = expression.And(t => accs.Contains(t.AccountId));
            }

            
            if (filter.MinValue != default)
                expression = expression.And(t => t.Value >= filter.MinValue);
            if (filter.MaxValue != default)
                expression = expression.And(t => t.Value <= filter.MaxValue);

            var query = _transactionRepository.FindByCondition(expression/*Запрос должен обязательно включать userId */, trackChanges: false);
            var totalCount = await query.CountAsync(token);

            var page = filter.Page < 1 ? 1 : filter.Page;
            const int pageSize = 10; 
            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(token);

            if (transactions is null)
            {
                return Result<PagedResponse<TransactionResponse>>.Failure(TransactionErrors.TransactionNotFound);
            }
            else
            {
                var transactionResponses = new List<TransactionResponse>();
                foreach (var transaction in transactions)
                {
                    transactionResponses.Add(transaction.ToTransactionResponse());
                }
                var responses = new PagedResponse<TransactionResponse>(transactionResponses, totalCount, filter.Page, pageSize);
                return Result<PagedResponse<TransactionResponse>>.Success(responses);
            }
        }

        public async Task<Result<List<TransactionResponse>>> GetTransactionsAnalyticsAsync(TransactionFilterParameters filter, CancellationToken token)
        {
            Expression<Func<Transaction, bool>> expression = t => true;

            //expression = expression.And(t => t.UserId == filter.UsertId);
            if (filter.StartDate.HasValue)
                expression = expression.And(t => t.TransactionDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                expression = expression.And(t => t.TransactionDate <= filter.EndDate.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim();
                expression = expression.And(t =>
                    t.Title.Contains(term) ||
                    t.Description.Contains(term) ||
                    t.Merchant.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(filter.Categories))
            {
                var cats = filter.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                            .Select(long.Parse)
                                            .ToList();
                expression = expression.And(t => cats.Contains(t.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(filter.Types))
            {
                var types = filter.Types.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .ToList();
                expression = expression.And(t => types.Contains(t.TransactionType.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(filter.Accounts))
            {
                var accs = filter.Accounts.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(long.Parse)
                                          .ToList();
                expression = expression.And(t => accs.Contains(t.AccountId));
            }


            if (filter.MinValue != default)
                expression = expression.And(t => t.Value >= filter.MinValue);
            if (filter.MaxValue != default)
                expression = expression.And(t => t.Value <= filter.MaxValue);

            var query = _transactionRepository.FindByCondition(expression, trackChanges: false);

            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(token);

            if (transactions is null)
            {
                return Result<List<TransactionResponse>>.Failure(TransactionErrors.TransactionNotFound);
            }
            else
            {
                var transactionResponses = new List<TransactionResponse>();
                foreach (var transaction in transactions)
                {
                    transactionResponses.Add(transaction.ToTransactionResponse());
                }
                return Result<List<TransactionResponse>>.Success(transactionResponses);
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
