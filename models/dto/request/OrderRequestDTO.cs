using System.ComponentModel.DataAnnotations;

namespace luchito_net.Models.Dto.Request
{
    /// <summary>
    /// Data transfer object for creating or updating an order.
    /// </summary>
    public class OrderRequestDto
    {
        /// <summary>
        /// The ID of the product to order.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "ProductId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
        public int ProductId { get; set; }

        /// <summary>
        /// The quantity of the product to order.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        /// <summary>
        /// The ID of the order state.
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "StateId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "StateId must be greater than 0.")]
        public int StateId { get; set; }

        /// <summary>
        /// Indicates whether the order is boxed.
        /// </summary>
        [Required(ErrorMessage = "IsBoxed is required.")]
        public bool IsBoxed { get; set; }
        /// <summary> The ID of the provider for the order.
        /// </summary>
        /// <example>1</example>
        public int? ProviderId { get; set; }
        /// <summary> Indicates whether the order is active.
        /// </summary>
        public bool? IsActive { get; set; }
    }
}