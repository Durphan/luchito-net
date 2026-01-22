using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;

namespace luchito_net.Models.Mappers;

public static class OrderMapper
{
    public static OrderResponseDto ToResponseDto(this Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            Product = order.Product.Name,
            Quantity = order.Quantity,
            CreatedAt = order.CreatedAt,
            State = order.State.Name,
            Provider = order.Provider?.Name ?? "Sin Proveedor",
            IsBoxed = order.IsBoxed,
            IsActive = order.IsActive
        };
    }

    public static Order ToEntity(this OrderRequestDto orderDto)
    {
        return new Order
        {
            ProductId = orderDto.ProductId,
            Quantity = orderDto.Quantity,
            StateId = orderDto.StateId,
            ProviderId = orderDto.ProviderId,
            IsBoxed = orderDto.IsBoxed,
            CreatedAt = DateTime.UtcNow,
            IsActive = orderDto.IsActive ?? true
        };
    }

    public static GetAllOrdersResponseDto ToGetAllOrdersResponseDto(this IEnumerable<Order> orders, int total, int page, int take)
    {
        return new GetAllOrdersResponseDto
        {
            Data = orders.Select(o => o.ToResponseDto()),
            Total = total,
            Page = page,
            Limit = take
        };
    }
}