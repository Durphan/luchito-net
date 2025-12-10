using System.ComponentModel.DataAnnotations;

namespace luchito_net.Models.Dto.Request
{
    /// <summary>
    /// Data transfer object for creating or updating a provider.
    /// </summary>
    public class ProviderRequestDto
    {
        /// <summary>
        /// The name of the provider.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the provider is a distributor (true) or wholesaler (false).
        /// </summary>
        /// 
        [Required(ErrorMessage = "IsDistributor is required.")]
        public bool IsDistributor { get; set; }

        /// <summary>
        /// Indicates if the provider is active. Defaults to true if not provided.
        /// </summary>
        [Required(ErrorMessage = "IsActive is required.")]
        public bool? IsActive { get; set; }
    }
}