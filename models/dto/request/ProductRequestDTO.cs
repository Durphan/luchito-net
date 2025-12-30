using System.ComponentModel.DataAnnotations;

namespace luchito_net.Models.Dto.Request
{
    public class ProductRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public required int CategoryId { get; set; }
    }
}