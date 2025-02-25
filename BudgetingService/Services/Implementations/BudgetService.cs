using FluentValidation;
using BudgetingService.Services.Interfaces;
using BudgetingService.Repositories.Interfaces;
using BudgetingService.Models.DTO.Requests;
using BudgetingService.Models.DTO.Responses;
using BudgetingService.Models.Errors;
using BudgetingService.Services.Mappings;

namespace BudgetingService.Services.Implementations
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _transactionRepository;
        private readonly IValidator<CreateBudgetRequest> _createTransactionRequestValidator;
        private readonly IValidator<UpdateBudgetRequest> _updateTransactionRequestValidator;

        public BudgetService(IBudgetRepository transactionRepository, IValidator<CreateBudgetRequest> createTransactionRequestValidator,
            IValidator<UpdateBudgetRequest> updateTransactionRequestValidator)
        {
            _transactionRepository = transactionRepository;
            _createTransactionRequestValidator = createTransactionRequestValidator;
            _updateTransactionRequestValidator = updateTransactionRequestValidator;
        }

        public async Task<Result<BudgetResponse>> GetTransactionByIdAsync(long id, CancellationToken token)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id, token);

            return transaction is null
                ? Result<BudgetResponse>.Failure(BudgetErrors.TransactionNotFound)
                : Result<BudgetResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<BudgetResponse>> GetTransactionByEmailAsync(string email, CancellationToken token)
        {
            var user = await _transactionRepository.GetByEmailAsync(email, token);

            return user is null
                ? Result<BudgetResponse>.Failure(BudgetErrors.TransactionNotFound)
                : Result<BudgetResponse>.Success(user.ToTransactionResponse());
        }

        public async Task<Result<BudgetResponse>> CreateTransactionAsync(CreateBudgetRequest createTransactionRequest,
            CancellationToken token)
        {
            var validationResult = await _createTransactionRequestValidator.ValidateAsync(createTransactionRequest, token);

            if (!validationResult.IsValid)
                return Result<BudgetResponse>.Failure(BudgetErrors.InvalidCredentials);

            var transaction = createTransactionRequest.ToTransaction();

            await _transactionRepository.AddAsync(transaction, token);

            return Result<BudgetResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<Result<BudgetResponse>> UpdateTransactionAsync(UpdateBudgetRequest updateTransactionRequest,
            CancellationToken token)
        {
            var validationResult = await _updateTransactionRequestValidator.ValidateAsync(updateTransactionRequest, token);

            if (!validationResult.IsValid)
                return Result<BudgetResponse>.Failure(BudgetErrors.InvalidCredentials);

            var transaction = updateTransactionRequest.ToTransaction();
            transaction = await _transactionRepository.UpdateAsync(transaction, token);

            return transaction is null
                ? Result<BudgetResponse>.Failure(BudgetErrors.TransactionNotFound)
                : Result<BudgetResponse>.Success(transaction.ToTransactionResponse());
        }

        public async Task<bool> DeleteTransactionAsync(long id, CancellationToken token)
        {
            await _transactionRepository.DeleteAsync(id, token);

            return true;
        }
    }
}
