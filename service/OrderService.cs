using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;

namespace luchito_net.Service
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {

        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<GetAllOrdersResponseDto> GetAllOrders(int page, int take, bool onlyActive = true)
        {
            (IEnumerable<Order> orders, int total) = await _orderRepository.GetAllOrders(page, take, onlyActive);
            return orders.ToGetAllOrdersResponseDto(total, page, take);
        }

        public async Task<OrderResponseDto> CreateOrder(OrderRequestDto orderDto)
        {
            var createdOrder = await _orderRepository.CreateOrder(orderDto.ToEntity());
            return createdOrder.ToResponseDto();
        }

        public async Task<OrderResponseDto> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            return order.ToResponseDto();
        }

        public async Task<OrderResponseDto> UpdateOrder(int id, OrderRequestDto orderDto)
        {
            var order = await _orderRepository.UpdateOrder(id, orderDto.ToEntity());
            return order.ToResponseDto();
        }

        public async Task<OrderResponseDto> DeleteOrder(int id)
        {
            var deletedOrder = await _orderRepository.DeleteOrder(id);
            return deletedOrder.ToResponseDto();
        }
    }
}