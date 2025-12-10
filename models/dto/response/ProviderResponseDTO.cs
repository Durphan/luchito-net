namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for provider response data.
    /// </summary>
    public class ProviderResponseDto
    {
        /// <summary>
        /// The unique identifier of the provider.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the provider.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the provider is a distributor (true) or wholesaler (false).
        /// </summary>
        public bool IsDistributor { get; set; }

        /// <summary>
        /// The name of the provider type (distributor or wholesaler).
        /// </summary>
        public string ProviderType => IsDistributor ? "Distribuidor" : "Mayorista";

        /// <summary>
        /// Indicates whether the provider is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}