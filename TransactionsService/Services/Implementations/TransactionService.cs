using FluentValidation;
using TransactionsService.Services.Interfaces;
using TransactionsService.Repositories.Interfaces;
using TransactionsService.Models.DTO.Requests;
using TransactionsService.Models.DTO.Responses;
using TransactionsService.Models.Errors;
using TransactionsService.Services.Mappings;

namespace TransactionsService.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IValidator<CreateTransactionRequest> _createTransactionRequestValidator;
        private readonly IValidator<UpdateTransactionRequest> _updateTransactionRequestValidator;

        public TransactionService(ITransactionRepository transactionRepository, IValidator<CreateTransactionRequest> createTransactionRequestValidator,
            IValidator<UpdateTransactionRequest> updateTransactionRequestValidator)
        {
            _transactionRepository = transactionRepository;
            _createTransactionRequestValidator = createTransactionRequestValidator;
            _updateTransactionRequestValidator = updateTransactionRequestValidator;
        }

        public async Task<Result<TransactionResponse>> GetTransactionByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, token);

            return transaction is null
                ? Result<TransactionResponse>.Failure(TransactionErrors.TransactionNotFound)
                : Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<TransactionResponse>> GetTransactionByEmailAsync(string email, CancellationToken token)
        {
            var user = await _transactionRepository.GetByEmailAsync(email, token);

            return user is null
                ? Result<TransactionResponse>.Failure(TransactionErrors.TransactionNotFound)
                : Result<TransactionResponse>.Success(user.ToTransactionResponse());
        }

        public async Task<Result<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest,
            CancellationToken token)
        {
            var validationResult = await _createTransactionRequestValidator.ValidateAsync(createTransactionRequest, token);

            if (!validationResult.IsValid)
                return Result<TransactionResponse>.Failure(TransactionErrors.InvalidCredentials);

            var transaction = createTransactionRequest.ToTransaction();

            await _transactionRepository.AddAsync(transaction, token);

            return Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<TransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest,
            CancellationToken token)
        {
            var validationResult = await _updateTransactionRequestValidator.ValidateAsync(updateTransactionRequest, token);

            if (!validationResult.IsValid)
                return Result<TransactionResponse>.Failure(TransactionErrors.InvalidCredentials);

            var transaction = updateTransactionRequest.ToTransaction();
            transaction = await _transactionRepository.UpdateAsync(transaction, token);

            return transaction is null
                ? Result<TransactionResponse>.Failure(TransactionErrors.TransactionNotFound)
                : Result<TransactionResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<bool> DeleteTransactionAsync(long id, CancellationToken token)
        {
            await _transactionRepository.DeleteAsync(id, token);

            return true;
        }
    }
}
