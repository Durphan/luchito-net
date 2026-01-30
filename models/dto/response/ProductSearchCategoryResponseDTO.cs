
using luchito_net.Models.Entity;
using luchito_net.Models.Mappers;

namespace luchito_net.Models.Dto.Response;

public class ProductSearchCategoryResponseDto(string category, int total, int page, int limit, IEnumerable<Product> data)
{
    public string Category { get; set; } = category;
    public int Total { get; set; } = total;
    public int Page { get; set; } = page;
    public int Limit { get; set; } = limit;
    public IEnumerable<ProductResponseDtoWithoutCategory> Data { get; set; } = data.Select(ProductMapper.ProductResponseDtoWithoutCategory);
}
