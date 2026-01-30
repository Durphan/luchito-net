namespace luchito_net.Models.Dto.Response
{
    public class ProductResponseDtoWithoutCategory(int id, string name, bool isActive)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;

        public bool IsActive { get; set; } = isActive;
    }
}