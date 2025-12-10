using Microsoft.AspNetCore.Mvc;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace luchito_net.Controllers
{
    [ApiController]
    [Route("api")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        /// <summary>
        /// Get all categories with optional filtering and pagination
        /// </summary>
        /// <param name="name">Filter categories by name (optional)</param>
        /// <param name="page">The page number to retrieve (default is 1)</param>
        /// <param name="take">The number of categories to take per page (default is 10)</param>
        /// <param name="onlyActive">Whether to include only active categories (default is true)</param>
        /// <returns>A paginated list of categories.</returns>
        [HttpGet("getCategories")]
        [SwaggerOperation(Summary = "Get all categories with optional filtering and pagination")]
        [ProducesResponseType(typeof(GetAllCategoriesResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<GetAllCategoriesResponseDto>> GetAllCategories(
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int take = 10,
            [FromQuery] bool onlyActive = true)
        {
            GetAllCategoriesResponseDto result = await _categoryService.GetAllCategories(name ?? string.Empty, page, take, onlyActive);
            return Ok(result);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <param name="categoryDto">The category data to create</param>
        /// <returns>The category that was created</returns>
        [HttpPost("createCategory")]
        [SwaggerOperation(Summary = "Create a new category")]
        [ProducesResponseType(typeof(CategoryResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryRequestDto categoryDto)
        {
            var result = await _categoryService.CreateCategory(categoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get category by ID
        /// </summary>
        /// <param name="id">The ID of the category to retrieve</param>
        /// <returns>The category with the specified ID</returns>
        [HttpGet("category/{id}")]
        [SwaggerOperation(Summary = "Get category by ID")]
        [ProducesResponseType(typeof(CategoryResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
        {
            var result = await _categoryService.GetCategoryById(id);
            return Ok(result);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="categoryDto">The updated category data</param>
        /// <returns>The updated category</returns>
        [HttpPut("updateCategory/{id}")]
        [SwaggerOperation(Summary = "Update an existing category")]
        [ProducesResponseType(typeof(CategoryResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] CategoryRequestDto categoryDto)
        {
            var result = await _categoryService.UpdateCategory(id, categoryDto);
            return Ok(result);
        }

        /// <summary>
        /// Delete a category by ID
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>The deleted category</returns>
        [HttpDelete("deleteCategory/{id}")]
        [SwaggerOperation(Summary = "Delete a category by ID")]
        [ProducesResponseType(typeof(CategoryResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<CategoryResponseDto>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return Ok(result);
        }
    }
}