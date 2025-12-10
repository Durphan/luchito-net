using System.ComponentModel.DataAnnotations;

namespace luchito_net.Models.Dto.Request
{
    /// <summary>
    /// Data transfer object for creating or updating a category.
    /// </summary>
    public class CategoryRequestDto
    {
        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the parent category for hierarchical structure. Null if it's a root category.
        /// </summary>
        /// <example>null</example>
        [Range(1, int.MaxValue, ErrorMessage = "ParentCategoryID must be greater than 0 if provided.")]
        public int? ParentCategoryID { get; set; }

        /// <summary>
        /// Indicates if the category is active. Defaults to true if not provided.
        /// </summary>
        public bool? IsActive { get; set; }
    }
}