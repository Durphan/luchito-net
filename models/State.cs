using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace luchito_net.Models
{
    [Table("State")]
    [Index(nameof(Name), IsUnique = true)]
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [InverseProperty("State")]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}