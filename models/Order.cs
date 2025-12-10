using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace luchito_net.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("ProductId")]
        public required int ProductId { get; set; }

        [Required]
        public virtual Product Product { get; set; } = null!;

        [Required]
        public required int Quantity { get; set; }

        [Required]
        public required bool IsBoxed { get; set; }

        [Required]
        public required DateTime OrderDate { get; set; }

        [Required]
        public required bool IsActive { get; set; } = true;

        [Required, ForeignKey("StateId")]
        public required int StateId { get; set; }

        [Required]
        public virtual State State { get; set; } = null!;

        [ForeignKey("ProviderId")]
        public int? ProviderId { get; set; }

        public virtual Provider? Provider { get; set; } = null!;
    }
}