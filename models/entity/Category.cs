using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace luchito_net.Models.Entity;

[Index(nameof(Name), IsUnique = true)]
[Table("category")]
public class Category
{
	[Key, Column("id")]
	public int Id { get; set; }
	[Required, Column("name")]
	public required string Name { get; set; }

	[Required, DefaultValue(true), Column("is_active")]
	public bool IsActive { get; set; }

	[ForeignKey("ParentCategory"), Column("parent_category_id")]
	public int? ParentCategoryID { get; set; }

	[ForeignKey("ParentCategoryID")]
	public virtual Category? ParentCategory { get; set; }

	[Required, Column("created_at")]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	[Column("updated_at")]
	public DateTime? UpdatedAt { get; set; } = null;

	[InverseProperty("Category")]
	public ICollection<Product>? Products { get; set; }

}
