using System.Runtime.CompilerServices;
using CaregoryAccountService.Models;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Enums;

namespace UsersService.Services.Mappings
{
    public static class CategoryMapping
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            (
                category.Id,
                category.UserId,
                category.CategoryName,
                category.CategoryType.ToString(),
                category.Icon
            );
        }

        public static Category ToCategory(this CreateCategoryRequest request)
        {
            return new Category
            {
                UserId = request.UserId,
                CategoryName = request.CategoryName,
                CategoryType = Enum.Parse<CategoryType>(request.CategoryType),
                Icon = request.Icon
            };
        }

        public static Category ToCategory(this UpdateCategoryRequest request)
        {
            return new Category
            {
                Id = request.Id,
                UserId = request.UserId,
                CategoryName = request.CategoryName,
                CategoryType = Enum.Parse<CategoryType>(request.CategoryType),
                Icon = request.Icon
            };
        }
    }

}
