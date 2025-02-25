using FluentValidation;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Repositories.Implementations;
using CaregoryAccountService.Repositories.Interfaces;
using CaregoryAccountService.Services.Interfaces;
using CaregoryAccountService.Services.Mappings;
using CaregoryAccountService.Services.Validators;

namespace CaregoryAccountService.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _profileRepository;
        private readonly IValidator<CreateCategoryRequest> _createProfileRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateProfileRequestValidator;

        public CategoryService(ICategoryRepository profileRepository, IValidator<CreateCategoryRequest> createProfileRequestValidator,
            IValidator<UpdateCategoryRequest> updateProfileRequestValidator)
        {
            _profileRepository = profileRepository;
            _createProfileRequestValidator = createProfileRequestValidator;
            _updateProfileRequestValidator = updateProfileRequestValidator;
        }

        public async Task<Result<CategoryResponse>> CreateProfileAsync(CreateCategoryRequest createProfileRequest, CancellationToken token)
        {
            var validationResult = await _createProfileRequestValidator.ValidateAsync(createProfileRequest, token);

            if (!validationResult.IsValid)
                return Result<CategoryResponse>.Failure(CategoryErrors.InvalidCredentials);
            var profile = createProfileRequest.ToProfile();

            await _profileRepository.AddAsync(profile, token);

            return Result<CategoryResponse>.Success(profile.ToProfileResponse());
        }

        public async Task<bool> DeleteProfileAsync(long id, CancellationToken token)
        {
            await _profileRepository.DeleteAsync(id, token);

            return true;
        }

        public async Task<Result<CategoryResponse>> GetProfileByIdAsync(long id, CancellationToken token)
        {
            var profile = await _profileRepository.GetByIdAsync(id, token);

            return profile is null
                ? Result<CategoryResponse>.Success(profile.ToProfileResponse())
                : Result<CategoryResponse>.Failure(CategoryErrors.ProfileNotFound);
        }

        public async Task<Result<CategoryResponse>> UpdateProfileAsync(UpdateCategoryRequest updateProfileRequest, CancellationToken token)
        {
            var validationResult = await _updateProfileRequestValidator.ValidateAsync(updateProfileRequest, token);

            if (!validationResult.IsValid)
                return Result<CategoryResponse>.Failure(CategoryErrors.InvalidCredentials);

            var profile = updateProfileRequest.ToProfile();
            profile = await _profileRepository.UpdateAsync(profile, token);

            return profile is null
                ? Result<CategoryResponse>.Failure(CategoryErrors.ProfileNotFound)
                : Result<CategoryResponse>.Success(profile.ToProfileResponse());
        }
    }
}
