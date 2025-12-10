using luchito_net.Models;

namespace luchito_net.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<(IEnumerable<Order> Orders, int Total)> GetAllOrders(int page, int take, bool onlyActive = true);

        Task<Order> CreateOrder(Order order);

        Task<Order> UpdateOrder(int id, Order order);

        Task<Order> GetOrderById(int id);

        Task<Order> DeleteOrder(int id);
    }
}