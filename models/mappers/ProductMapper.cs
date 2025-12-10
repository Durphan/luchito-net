using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;

namespace luchito_net.Models.Mappers;

public static class ProductMapper
{
    public static ProductResponseDto ToResponseDto(this Product product)
    {
        return new ProductResponseDto(product.Id, product.Name, product.Category.Name, product.IsActive);
    }

    public static Product ToEntity(this ProductRequestDto productDto)
    {
        return new Product
        {
            Name = productDto.Name,
            CategoryId = productDto.CategoryId,
            IsActive = true
        };
    }

    public static ProductResponseDto EntityToResponseDTO(this Product product)
    {
        return new ProductResponseDto(product.Id, product.Name, product.Category.Name, product.IsActive);
    }

    public static ProductWithoutCategory EntityToWithoutCategoryResponseDTO(this Product product)
    {
        return new ProductWithoutCategory(product.Id, product.Name, product.IsActive);
    }

    public static ProductSearchResponseDto ToSearchResponseDto(this IEnumerable<Product> products, string query, int total, int page, int pageSize)
    {
        return new ProductSearchResponseDto(query, total, page, pageSize, products);
    }

    public static ProductSearchCategoryResponseDto ToSearchCategoryResponseDto(this IEnumerable<Product> products, string category, int total, int page, int pageSize)
    {
        return new ProductSearchCategoryResponseDto(category, total, page, pageSize, products);
    }
}