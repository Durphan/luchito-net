using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace luchito_net.Models
{
    [Table("Provider")]
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required bool IsDistributor { get; set; }
        [Required]
        public required bool IsActive { get; set; } = true;

        [InverseProperty("Provider")]
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}