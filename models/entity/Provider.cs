using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace luchito_net.Models.Entity;

[Table("provider")]
public class Provider
{
    [Key, Column("id")]
    public int Id { get; set; }
    [Required, Column("name")]
    public required string Name { get; set; }
    [Required, Column("is_distributor")]
    public required bool IsDistributor { get; set; }
    [Required, Column("is_active")]
    public required bool IsActive { get; set; } = true;

    [InverseProperty("Provider")]
    public virtual ICollection<Order> Orders { get; set; } = [];
}
