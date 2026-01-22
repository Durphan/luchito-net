using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace luchito_net.Models
{
    [Table("product")]
    public class Product
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, Column("name")]
        public required string Name { get; set; }


        [Required, Column("is_active")]
        public required bool IsActive { get; set; } = true;


        [Required, ForeignKey("category_id"), Column("category_id")]
        public required int CategoryId { get; set; }

        [Required]
        public virtual Category Category { get; set; } = null!;

        [InverseProperty("Product")]
        public virtual ICollection<Order>? Orders { get; set; }

    }
}