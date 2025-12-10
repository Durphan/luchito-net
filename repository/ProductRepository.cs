using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Errors;
using Microsoft.EntityFrameworkCore.Storage;

namespace luchito_net.Repository
{
    public class ProductRepository(InitializeDatabase _context, ILogger<ProductRepository> _logger) : IProductRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<ProductRepository> _logger = _logger;

        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                await _context.Set<Product>().AddAsync(product);
                await _context.SaveChangesAsync();
                Product productCreated = await _context.Set<Product>().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == product.Id)
                    ?? throw new NotFoundException("Failed to retrieve the created product.");
                return productCreated;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in CreateProduct for product {ProductName}", product.Name);
                throw;
            }
        }

        public async Task<Product> DeleteProduct(int id)
        {
            try
            {
                Product productToDelete = await _context.Set<Product>().FindAsync(id) ?? throw new NotFoundException($"Product with ID {id} not found.");
                _context.Set<Product>().Remove(productToDelete);
                await _context.SaveChangesAsync();
                return productToDelete;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in DeleteProduct for product ID {ProductId}", id);
                throw;
            }
        }


        public async Task<Product> GetProductById(int id)
        {
            try
            {
                return await _context.Set<Product>().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException($"Product with ID {id} not found.");
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in GetProductById for product ID {ProductId}", id);
                throw;
            }
        }



        public async Task<Product> UpdateProduct(Product product)
        {
            try
            {
                Product existingProduct = await _context.Set<Product>().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == product.Id)
                ?? throw new NotFoundException($"Product with ID {product.Id} not found.");
                existingProduct.Name = product.Name;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.IsActive = product.IsActive;
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in UpdateProduct for product ID {ProductId}", product.Id);
                throw;
            }
        }


        public async Task<(IEnumerable<Product> products, int total)> GetProductsByCategoryId(int categoryId, int page, int limit, bool onlyActive = true)
        {
            try
            {
                IEnumerable<Product> products = await _context.Set<Product>()
                    .Where(p => p.CategoryId == categoryId && (!onlyActive || p.IsActive))
                    .Skip((page - 1) * limit)
                    .Include(p => p.Category)
                    .Take(limit)
                    .ToListAsync();
                int total = await _context.Set<Product>().CountAsync(p => p.CategoryId == categoryId && (!onlyActive || p.IsActive));
                return (products, total);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in GetProductsByCategoryId for category ID {CategoryId}, page {Page}, limit {Limit}", categoryId, page, limit);
                throw;
            }
        }

        public async Task<(IEnumerable<Product> products, int total)> GetProductsByName(string name, int page, int limit, bool onlyActive = true)
        {
            try
            {
                IEnumerable<Product> products = await _context.Set<Product>()
                    .Where(p => p.Name.Contains(name) && (!onlyActive || p.IsActive))
                    .Skip((page - 1) * limit)
                    .Include(p => p.Category)
                    .Take(limit)
                    .ToListAsync();
                int total = await _context.Set<Product>().CountAsync(p => p.Name.Contains(name) && (!onlyActive || p.IsActive));
                return (products, total);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in GetProductsByName for query {Query}, page {Page}, limit {Limit}", name, page, limit);
                throw;
            }
        }

        public async Task<(IEnumerable<Product> products, int total)> GetAllProducts(int page, int limit, bool onlyActive = true)
        {
            try
            {
                IEnumerable<Product> products = await _context.Set<Product>()
                    .Where(p => !onlyActive || p.IsActive)
                    .Skip((page - 1) * limit)
                    .Include(p => p.Category)
                    .Take(limit)
                    .ToListAsync();
                int total = await _context.Set<Product>().CountAsync(p => !onlyActive || p.IsActive);
                return (products, total);
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex, "Error in GetAllProducts with page {Page}, limit {Limit}, onlyActive {OnlyActive}", page, limit, onlyActive);
                throw;
            }
        }
    }
}