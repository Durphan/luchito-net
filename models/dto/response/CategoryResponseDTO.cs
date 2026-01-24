using luchito_net.Models.Entity;

namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for category response data.
    /// </summary>
    public class CategoryResponseDto(Category category)
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int Id { get; set; } = category.Id;

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; } = category.Name;

        /// <summary>
        /// Indicates whether the category is active.
        /// </summary>
        public bool IsActive { get; set; } = category.IsActive;

        /// <summary>
        /// Indicates whether the category has subcategories.
        /// </summary>
        public bool IsFather { get; set; } = category.Subcategories != null && category.Subcategories.Count != 0;

        /// <summary>
        /// The ID of the parent category for hierarchical structure. Null if it's a root category.
        /// </summary>
        public int? ParentCategoryID { get; set; } = category.ParentCategoryID;


        /// <summary>
        /// The date and time when the category was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = category.CreatedAt;

        /// <summary>
        /// The date and time when the category was last updated. Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; } = category.UpdatedAt;

        public override string ToString()
        {
            return $"CategoryResponseDto {{ Id = {Id}, Name = {Name}, IsActive = {IsActive}, IsFather = {IsFather}, ParentCategoryID = {ParentCategoryID}, CreatedAt = {CreatedAt}, UpdatedAt = {UpdatedAt} }}";
        }
    }
}