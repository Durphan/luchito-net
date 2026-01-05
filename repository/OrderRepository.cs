using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Errors;

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
                Order createdOrder = (await _context.Set<Order>().AddAsync(order)).Entity;
                await _context.SaveChangesAsync();
                createdOrder = await GetOrderById(createdOrder.Id);
                return createdOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateOrder for order with ProductId {ProductId}", order.ProductId);
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Order> DeleteOrder(int id)
        {
            try
            {
                Order orderToDelete = await GetOrderById(id);
                orderToDelete.IsActive = false;
                _context.Set<Order>().Update(orderToDelete);
                await _context.SaveChangesAsync();
                return orderToDelete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteOrder for order ID {OrderId}", id);
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                return await _context.Set<Order>()
                    .Include(o => o.Product)
                    .Include(o => o.State)
                    .Include(o => o.Provider)
                    .FirstOrDefaultAsync(o => o.Id == id) ?? throw new NotFoundException($"Order with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrderById for order ID {OrderId}", id);
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<(IEnumerable<Order> Orders, int Total)> GetAllOrders(int page, int take, bool onlyActive = true)
        {
            try
            {
                IQueryable<Order> query = _context.Set<Order>()
                    .OrderBy(o => o.OrderDate)
                    .Include(o => o.Product)
                    .Include(o => o.State)
                    .Include(o => o.Provider);
                if (onlyActive)
                {
                    query = query.Where(o => o.IsActive);
                }
                IEnumerable<Order> orders = await query
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync();
                int total = await _context.Set<Order>()
                    .Where(o => !onlyActive || o.IsActive)
                    .CountAsync();
                return (orders, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllOrders with page {Page}, take {Take}, onlyActive {OnlyActive}", page, take, onlyActive);
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Order> UpdateOrder(int id, Order order)
        {
            try
            {
                Order existingOrder = await GetOrderById(id);
                existingOrder.ProductId = order.ProductId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.StateId = order.StateId;
                existingOrder.ProviderId = order.ProviderId;
                existingOrder.IsBoxed = order.IsBoxed;
                existingOrder.IsActive = order.IsActive;
                _context.Set<Order>().Update(existingOrder);
                await _context.SaveChangesAsync();
                Order newOrder = await GetOrderById(id);
                return newOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateOrder for order ID {OrderId}", id);
                throw new DatabaseException(ex.Message, ex);
            }
        }
    }
}