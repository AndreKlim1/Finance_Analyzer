using Asp.Versioning;
using CaregoryAccountService.Models.DTO.Requests;
using CaregoryAccountService.Models.DTO.Responses;
using CaregoryAccountService.Models.Errors;
using CaregoryAccountService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CaregoryAccountService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetCategoryByIdAsync(int id, CancellationToken token)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id, token);

            return result.Match<ActionResult<CategoryResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error)); 
                
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategoriesAsync(CancellationToken token)
        {
            var result = await _categoryService.GetCategoriesAsync(token);

            return result.Match<ActionResult<List<CategoryResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));

        }

        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategoriesByUserIdAsync(long userId, CancellationToken token)
        {
            var result = await _categoryService.GetCategoriesByUserIdAsync(userId, token);

            return result.Match<ActionResult<List<CategoryResponse>>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => NotFound(error));

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest, CancellationToken token)
        {
            var result = await _categoryService.CreateCategoryAsync(createCategoryRequest, token);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest,
            CancellationToken token)
        {
            var result = await _categoryService.UpdateCategoryAsync(updateCategoryRequest, token);

            return result.Match<ActionResult<CategoryResponse>>(
                onSuccess: () => Ok(result.Value),
                onFailure: error => BadRequest(error));
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategoryAsync(long id, CancellationToken token)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id, token);

            return isDeleted ? NoContent() : BadRequest();
        }
    }
}
