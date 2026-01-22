using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace luchito_net.Models
{
    [Table("order")]
    public class Order
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, ForeignKey("ProductId"), Column("product_id")]
        public required int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;

        [Required, Column("quantity")]
        public required int Quantity { get; set; }

        [Required, Column("is_boxed")]
        public required bool IsBoxed { get; set; }

        [Required, Column("created_at")]
        public required DateTime CreatedAt { get; set; }

        [Required, Column("is_active")]
        public bool IsActive { get; set; } = true;

        [ForeignKey("StateId"), Column("state_id")]
        public int StateId { get; set; }

        public virtual State State { get; set; } = null!;

        [ForeignKey("ProviderId"), Column("provider_id")]
        public int? ProviderId { get; set; }

        public virtual Provider? Provider { get; set; } = null!;
    }
}