namespace luchito_net.Models.Dto.Response
{
    public class ProductResponseDto(int id, string name, string category, bool isActive)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;

        public bool IsActive { get; set; } = isActive;

        public string Category { get; set; } = category;
    }
}