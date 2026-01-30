namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for paginated category list response.
    /// </summary>
    public class CategoriesPaginatedResponseDto
    {
        /// <summary>
        /// The total number of categories available.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of items per page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Indicates if there is a next page available.
        /// </summary>
        public bool HasNext => (Page * Limit) < Total;

        /// <summary>
        /// Indicates if there is a previous page available.
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// The list of categories for the current page.
        /// </summary>
        public IEnumerable<CategoryResponseDto> Data { get; set; } = new List<CategoryResponseDto>();
    }
}