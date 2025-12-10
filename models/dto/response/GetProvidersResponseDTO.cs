using Xunit.Sdk;

namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for paginated provider list response.
    /// </summary>
    public class GetProvidersResponseDto
    {

        /// <summary>
        /// The search term used to filter providers.
        /// </summary>
        public string Search { get; set; } = string.Empty;

        /// <summary>
        /// The field used to filter providers by state.
        /// </summary>
        public string StateFiltered { get; set; } = string.Empty;

        /// <summary>
        /// The total number of providers available.
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
        /// The list of providers for the current page.
        /// </summary>
        public IEnumerable<ProviderResponseDto> Data { get; set; } = new List<ProviderResponseDto>();
    }
}