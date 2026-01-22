namespace luchito_net.Models.Dto.Response
{
    /// <summary>
    /// Data transfer object for order response data.
    /// </summary>
    public class OrderResponseDto
    {
        /// <summary>
        /// The unique identifier of the order.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the product ordered.
        /// </summary>
        public required string Product { get; set; }

        /// <summary>
        /// The quantity of the product ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The date and time when the order was placed.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The name of the order state.
        /// </summary>
        public required string State { get; set; }

        /// <summary>
        /// The name of the provider.
        /// </summary>
        public required string Provider { get; set; }

        /// <summary>
        /// Indicates whether the order is boxed.
        /// </summary>
        public required bool IsBoxed { get; set; }

        /// <summary>
        /// Gets the unit type based on whether the order is boxed.
        /// </summary>
        public string UnitType => IsBoxed ? "Cajas" : "Unidades";

        /// <summary>
        /// Indicates whether the order is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}