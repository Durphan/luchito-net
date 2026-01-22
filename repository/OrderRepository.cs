using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;

namespace luchito_net.Repository
{
    public class OrderRepository(InitializeDatabase _context, ILogger<OrderRepository> logger) : IOrderRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<OrderRepository> _logger = logger;

        public async Task<Order> CreateOrder(Order order)
        {
            try
            {
                var createdOrder = await _context.Order.AddAsync(order);
                await _context.SaveChangesAsync();
                return await GetOrderById(createdOrder.Entity.Id);
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in CreateOrder for order with ProductId {ProductId}", order.ProductId);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Order> DeleteOrder(int id)
        {
            try
            {
                Order orderToDelete = await GetOrderById(id);
                orderToDelete.IsActive = false;
                await _context.SaveChangesAsync();
                return orderToDelete;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in DeleteOrder for order ID {OrderId}", id);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Order
                .Include(o => o.Product)
                .Include(o => o.State)
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(o => o.Id == id) ?? throw new Exception($"Order with ID {id} not found.");
        }

        public async Task<(IEnumerable<Order> Orders, int Total)> GetAllOrders(int page, int take, bool onlyActive = true)
        {

            var query = await _context.Order
                .Where(o => !onlyActive || o.IsActive)
                .OrderBy(o => o.CreatedAt)
                .Include(o => o.Product)
                .Include(o => o.State)
                .Include(o => o.Provider)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return (query, await _context.Order
                .Where(o => !onlyActive || o.IsActive)
                .CountAsync());

        }

        public async Task<Order> UpdateOrder(int id, Order order)
        {
            try
            {
                var existingOrder = await GetOrderById(id);
                existingOrder.ProductId = order.ProductId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.StateId = order.StateId;
                existingOrder.ProviderId = order.ProviderId;
                existingOrder.IsBoxed = order.IsBoxed;
                existingOrder.IsActive = order.IsActive;
                await _context.SaveChangesAsync();
                return await GetOrderById(id);
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in UpdateOrder for order ID {OrderId}", id);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}