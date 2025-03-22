using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Implementations;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Services.Validators;
using UsersService.Services.Mappings;
using CaregoryAccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace CaregoryAccountService.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;

        public CategoryService(ICategoryRepository categoryRepository, IValidator<CreateCategoryRequest> createCategoryRequestValidator,
            IValidator<UpdateCategoryRequest> updateCategoryRequestValidator)
        {
            _categoryRepository = categoryRepository;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
        }

        public async Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken token)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest, token);

            if (!validationResult.IsValid)
                return Result<CategoryResponse>.Failure(CategoryErrors.InvalidCredentials);
            var category = createCategoryRequest.ToCategory();

            await _categoryRepository.AddAsync(category, token);

            return Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

        public async Task<bool> DeleteCategoryAsync(long id, CancellationToken token)
        {
            await _categoryRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken token)
        {
            var categories = await _categoryRepository.FindAll(true).ToListAsync();
            if(categories is null)
            {
                return Result<List<CategoryResponse>>.Failure(CategoryErrors.ProfileNotFound);
            }
            else
            {
                var responses = new List<CategoryResponse>();
                foreach (var category in categories)
                {
                    responses.Add(category.ToCategoryResponse());
                }
                return Result<List<CategoryResponse>>.Success(responses);
            }
            
        }

        public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(long id, CancellationToken token)
        {
            var category = await _categoryRepository.GetByIdAsync(id, token);

            return category is null
                ? Result<CategoryResponse>.Failure(CategoryErrors.ProfileNotFound)
                : Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken token)
        {
            var validationResult = await _updateCategoryRequestValidator.ValidateAsync(updateCategoryRequest, token);

            if (!validationResult.IsValid)
                return Result<CategoryResponse>.Failure(CategoryErrors.InvalidCredentials);

            var category = updateCategoryRequest.ToCategory();
            category = await _categoryRepository.UpdateAsync(category, token);

            return category is null
                ? Result<CategoryResponse>.Failure(CategoryErrors.ProfileNotFound)
                : Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

    }
}
