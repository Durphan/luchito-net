using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using Npgsql;
using Dapper;
using System.Data;
using luchito_net.Models.Entity;

namespace luchito_net.Repository
{
    public class ProductRepository(InitializeDatabase _context, ILogger<ProductRepository> _logger, IConfiguration configuration) : IProductRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<ProductRepository> _logger = _logger;

        private readonly IConfiguration _configuration = configuration;

        private NpgsqlConnection Connection() => new(_configuration.GetConnectionString("DockerPostgreSql"));


        public async Task<Product> CreateProduct(Product product)
        {
            try
            {
                await _context.Product.AddAsync(product);
                await _context.SaveChangesAsync();
                return await GetProductById(product.Id);
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in CreateProduct for product {ProductName}", product.Name);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Product> DeleteProduct(int id)
        {
            try
            {
                Product productToDelete = await GetProductById(id);
                productToDelete.IsActive = false;
                await _context.SaveChangesAsync();
                return productToDelete;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in DeleteProduct for product ID {ProductId}", id);
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<Product> GetProductById(int id)
        {
            return await _context.Product.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception($"Product with ID {id} not found.");
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            try
            {
                var existingProduct = await GetProductById(product.Id)
                ?? throw new Exception($"Product with ID {product.Id} not found.");
                existingProduct.Name = product.Name;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.IsActive = product.IsActive;
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error in UpdateProduct for product ID {ProductId}", product.Id);
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<(IEnumerable<Product> products, int total)> GetProductsByCategoryId(int categoryId, int page, int limit, bool onlyActive = true)
        {

            using var db = Connection();

            db.Open();

            var products = await db.QueryAsync<Product>(@"with recursive category_cte as  (

select c.id from category c
where c.id = @CategoryId

union all

select c.id from category_cte cte
join category c on c.parent_category_id = cte.id
)


select p.* from product p
where p.category_id  in (select cte.id from category_cte cte)

", new { CategoryId = categoryId }, commandType: CommandType.Text);
            return (products, products.Count());
        }

        public async Task<(IEnumerable<Product> products, int total)> GetProductsByName(string name, int page, int limit, bool onlyActive = true)
        {

            var products = await _context.Product
                .Where(p => p.Name.Contains(name) && (!onlyActive || p.IsActive))
                .Skip((page - 1) * limit)
                .Include(p => p.Category)
                .Take(limit)
                .ToListAsync();
            return (products, await _context.Product.CountAsync(p => p.Name.Contains(name) && (!onlyActive || p.IsActive)));

        }

        public async Task<(IEnumerable<Product> products, int total)> GetAllProducts(int page, int limit, bool onlyActive = true)
        {

            IEnumerable<Product> products = await _context.Product
                .Where(p => !onlyActive || p.IsActive)
                .Skip((page - 1) * limit)
                .Include(p => p.Category)
                .Take(limit)
                .ToListAsync();
            return (products, await _context.Product.CountAsync(p => !onlyActive || p.IsActive));
        }
    }
}