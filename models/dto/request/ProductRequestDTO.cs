using System.ComponentModel.DataAnnotations;

namespace luchito_net.Models.Dto.Request
{
    public class ProductRequestDto(
        string Name,
        int CategoryId
    )
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = Name;

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; } = CategoryId;
    }
}