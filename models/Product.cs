using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace luchito_net.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }


        [Required]
        public required bool IsActive { get; set; } = true;


        [Required, ForeignKey("CategoryId")]
        public required int CategoryId { get; set; }

        [Required]
        public virtual Category Category { get; set; } = null!;

        [InverseProperty("Product")]
        public virtual ICollection<Order>? Orders { get; set; }

    }
}