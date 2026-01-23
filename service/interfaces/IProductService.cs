using luchito_net.Models.Dto.Response;
using luchito_net.Models.Dto.Request;


namespace luchito_net.Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductSearchResponseDto> SearchProductsByName(string query, int page, int pageSize, bool onlyActive);
        Task<ProductSearchCategoryResponseDto> SearchProductsByCategory(int idCategory, int page, int pageSize, bool onlyActive);
        Task<ProductResponseDto> GetProductById(int id);
        Task<ProductResponseDto> CreateProduct(ProductRequestDto product);
        Task<ProductResponseDto> UpdateProduct(int id, ProductRequestDto product);
        Task<ProductResponseDto> DeleteProduct(int id);
    }
}