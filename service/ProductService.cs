using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;

namespace luchito_net.Service
{
    public class ProductService(IProductRepository productRepository, ICategoryService categoryService) : IProductService
    {


        private readonly IProductRepository _productRepository = productRepository;

        private readonly ICategoryService _categoryService = categoryService;

        public async Task<ProductSearchResponseDto> SearchProductsByName(string query, int page, int pageSize, bool onlyActive)
        {
            (IEnumerable<Product> products, int total) = await _productRepository.GetProductsByName(query, page, pageSize, onlyActive);
            return products.ToSearchResponseDto(query, total, page, pageSize);
        }

        public async Task<ProductSearchCategoryResponseDto> SearchProductsByCategory(int idCategory, int page, int pageSize, bool onlyActive)
        {
            (IEnumerable<Product> products, int total) = await _productRepository.GetProductsByCategoryId(idCategory, page, pageSize, onlyActive);
            string category = (await _categoryService.GetCategoryById(idCategory)).Name;
            return products.ToSearchCategoryResponseDto(category, total, page, pageSize);
        }

        public async Task<ProductResponseDto> GetProductById(int id)
        {
            Product product = await _productRepository.GetProductById(id);
            return product.ToResponseDto();
        }

        public async Task<ProductResponseDto> AddProduct(ProductRequestDto product)
        {
            Product createdProduct = await _productRepository.CreateProduct(product.ToEntity());
            return createdProduct.ToResponseDto();
        }

        public async Task<ProductResponseDto> UpdateProduct(int id, ProductRequestDto product)
        {
            product.Name = NameNormalizer.Normalize(product.Name);
            Product entity = product.ToEntity();
            entity.Id = id;
            Product updatedProduct = await _productRepository.UpdateProduct(entity);
            return updatedProduct.ToResponseDto();
        }

        public async Task<ProductResponseDto> DeleteProduct(int id)
        {
            Product deletedProduct = await _productRepository.DeleteProduct(id);
            return deletedProduct.ToResponseDto();
        }
    }
}