using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;

namespace luchito_net.Service.Interfaces
{
    public interface IOrderService
    {
        Task<GetAllOrdersResponseDto> GetAllOrders(int page, int take, bool onlyActive = true);

        Task<OrderResponseDto> CreateOrder(OrderRequestDto orderDto);

        Task<OrderResponseDto> GetOrderById(int id);

        Task<OrderResponseDto> UpdateOrder(int id, OrderRequestDto orderDto);

        Task<OrderResponseDto> DeleteOrder(int id);
    }
}