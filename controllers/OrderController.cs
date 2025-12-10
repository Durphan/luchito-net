using Microsoft.AspNetCore.Mvc;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace luchito_net.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        /// <summary>
        /// Retrieves a paginated list of orders.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="take">The number of orders to take per page (default is 10).</param>
        /// <param name="onlyActive">Whether to include only active orders (default is true).</param>
        /// <returns>A paginated list of orders.</returns>
        [HttpGet("getOrders")]
        [SwaggerOperation(Summary = "Get all orders with pagination")]
        [ProducesResponseType(typeof(GetAllOrdersResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<GetAllOrdersResponseDto>> GetAllOrders(
            [FromQuery] int page = 1,
            [FromQuery] int take = 10,
            [FromQuery] bool onlyActive = true)
        {
            var result = await _orderService.GetAllOrders(page, take, onlyActive);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDto">The order data transfer object containing order details.</param>
        /// <returns>The created order.</returns>
        [HttpPost("createOrder")]
        [SwaggerOperation(Summary = "Create a new order")]
        [ProducesResponseType(typeof(OrderResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] OrderRequestDto orderDto)
        {
            var result = await _orderService.CreateOrder(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
        }


        /// <summary>
        /// Get an order by ID
        /// </summary>
        /// <param name="id">The ID of the order to retrieve</param>
        /// <returns>The order with the specified ID</returns>
        [HttpGet("order/{id}")]
        [SwaggerOperation(Summary = "Get an order by ID")]
        [ProducesResponseType(typeof(OrderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            return Ok(result);
        }
        /// <summary>
        /// Update an existing order
        /// </summary>
        /// <param name="id">The ID of the order to update</param>
        /// <param name="orderDto">The updated order data</param>
        /// <returns>The updated order</returns>
        [HttpPut("updateOrder/{id}")]
        [SwaggerOperation(Summary = "Update an existing order")]
        [ProducesResponseType(typeof(OrderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrder(int id, [FromBody] OrderRequestDto orderDto)
        {
            var result = await _orderService.UpdateOrder(id, orderDto);
            return Ok(result);
        }

        /// <summary>
        /// Delete an order by ID
        /// </summary>
        /// <param name="id">The ID of the order to delete</param>
        /// <returns>The deleted order</returns>
        [HttpDelete("deleteOrder/{id}")]
        [SwaggerOperation(Summary = "Delete an order by ID")]
        [ProducesResponseType(typeof(OrderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<OrderResponseDto>> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            return Ok(result);
        }
    }
}