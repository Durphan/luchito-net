

using luchito_net.Models.Mappers;

namespace luchito_net.Models.Dto.Response
{
    public class ProductSearchResponseDto(string search, int total, int page, int limit, IEnumerable<Product> data)
    {
        public string Search { get; set; } = search;
        public int Total { get; set; } = total;
        public int Page { get; set; } = page;
        public int Limit { get; set; } = limit;
        public IEnumerable<ProductResponseDto> Data { get; set; } = data.Select(ProductMapper.EntityToResponseDTO);
    }


}