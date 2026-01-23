using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace luchito_net.Models.Entity;

[Table("state")]
[Index(nameof(Name), IsUnique = true)]
public class State
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [InverseProperty("State")]
    public virtual ICollection<Order>? Orders { get; set; }
}
