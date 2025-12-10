using Microsoft.AspNetCore.Mvc;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace luchito_net.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Search products by name with pagination and active status filter
        /// </summary>
        /// <param name="query">The search query for product names</param>
        /// <param name="page">The page number for pagination</param>
        /// <param name="pageSize">The number of items per page for pagination</param>
        /// <param name="onlyActive">Filter to include only active products</param>
        /// <returns>The search results including products matching the query</returns>
        [HttpGet("getProducts")]
        [SwaggerOperation(Summary = "Search products by name with pagination and active status filter")]
        [ProducesResponseType(typeof(ProductSearchResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProductSearchResponseDto>> SearchProductsByName(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool onlyActive = true)
        {
            var result = await _productService.SearchProductsByName(query, page, pageSize, onlyActive);
            return Ok(result);
        }

        [HttpGet("getProducts/category/{idCategory}")]
        [SwaggerOperation(Summary = "Search products by category with pagination and active status filter")]
        [ProducesResponseType(typeof(ProductSearchCategoryResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProductSearchCategoryResponseDto>> SearchProductsByCategory(
            int idCategory,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool onlyActive = true)
        {
            var result = await _productService.SearchProductsByCategory(idCategory, page, pageSize, onlyActive);
            return Ok(result);
        }

        [HttpGet("product/{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            var result = await _productService.GetProductById(id);
            return Ok(result);
        }

        [HttpPost("createProduct")]
        public async Task<ActionResult<ProductResponseDto>> AddProduct([FromBody] ProductRequestDto product)
        {
            var result = await _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }

        [HttpPut("updateProduct/{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(int id, [FromBody] ProductRequestDto product)
        {
            var result = await _productService.UpdateProduct(id, product);
            return Ok(result);
        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<ActionResult<ProductResponseDto>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            return Ok(result);
        }
    }
}