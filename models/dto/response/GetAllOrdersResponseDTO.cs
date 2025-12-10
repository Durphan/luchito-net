namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for paginated order list response.
    /// </summary>
    public class GetAllOrdersResponseDto
    {
        /// <summary>
        /// The total number of orders available.
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
        /// The list of orders for the current page.
        /// </summary>
        public IEnumerable<OrderResponseDto> Data { get; set; } = [];
    }
}