namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for category response data.
    /// </summary>
    public class CategoryResponseDto
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the category is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The ID of the parent category for hierarchical structure. Null if it's a root category.
        /// </summary>
        public int? ParentCategoryID { get; set; }


        /// <summary>
        /// The date and time when the category was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the category was last updated. Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

    }
}