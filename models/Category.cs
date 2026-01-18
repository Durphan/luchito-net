using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace luchito_net.Models
{
	[Index(nameof(Name), IsUnique = true)]
	[Table("Category")]
	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public required string Name { get; set; }

		[Required, DefaultValue(true)]
		public bool IsActive { get; set; }

		[ForeignKey("ParentCategory")]
		public int? ParentCategoryID { get; set; }

		public virtual Category? ParentCategory { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; } = null;

		[InverseProperty("Category")]
		public ICollection<Product>? Products { get; set; }

		[InverseProperty("ParentCategory")]
		public ICollection<Category>? Subcategories { get; set; }
	}
}