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
using CategoryAccountService.Messaging.Kafka;
using CategoryAccountService.Messaging.DTO;

namespace CaregoryAccountService.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IKafkaProducer _kafkaProducer;

        public CategoryService(ICategoryRepository categoryRepository, IKafkaProducer kafkaProducer)
        {
            _categoryRepository = categoryRepository;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<Result<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken token)
        {

            var category = createCategoryRequest.ToCategory();

            await _categoryRepository.AddAsync(category, token);

            return Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

        public async Task<bool> DeleteCategoryAsync(long id, CancellationToken token)
        {
            var category = await _categoryRepository.GetByIdAsync(id, token);
            if (category != null && category.UserId!=null)
            {
                await _categoryRepository.DeleteAsync(id, token);
                var deletionEvent = new DeletionEvent("category-deleted", id);
                await _kafkaProducer.ProduceAsync("deletion-events", id.ToString(), deletionEvent);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Result<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken token)
        {
            var categories = await _categoryRepository.FindAll(true).ToListAsync();
            if(categories is null)
            {
                return Result<List<CategoryResponse>>.Failure(CategoryErrors.CategoryNotFound);
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

        public async Task<Result<List<CategoryResponse>>> GetCategoriesByUserIdAsync(long userId, CancellationToken token)
        {
            var categories = await _categoryRepository.FindByCondition(l => l.UserId == userId, true).ToListAsync();
            if (categories is null)
            {
                return Result<List<CategoryResponse>>.Failure(CategoryErrors.CategoryNotFound);
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
                ? Result<CategoryResponse>.Failure(CategoryErrors.CategoryNotFound)
                : Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

        public async Task<Result<CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest, CancellationToken token)
        {

            var category = updateCategoryRequest.ToCategory();
            category = await _categoryRepository.UpdateAsync(category, token);

            return category is null
                ? Result<CategoryResponse>.Failure(CategoryErrors.CategoryNotFound)
                : Result<CategoryResponse>.Success(category.ToCategoryResponse());
        }

    }
}
