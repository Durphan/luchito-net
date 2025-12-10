using luchito_net.Models;

namespace luchito_net.Repository.Interfaces
{
    public interface IProductRepository
    {

        Task<Product> CreateProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task<Product> DeleteProduct(int id);

        Task<Product> GetProductById(int id);

        Task<(IEnumerable<Product> products, int total)> GetAllProducts(int page, int limit, bool onlyActive = true);

        Task<(IEnumerable<Product> products, int total)> GetProductsByName(string name, int page, int limit, bool onlyActive = true);

        Task<(IEnumerable<Product> products, int total)> GetProductsByCategoryId(int categoryId, int page, int limit, bool onlyActive = true);
    }
}