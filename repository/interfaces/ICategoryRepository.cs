using luchito_net.Models;
using luchito_net.Models.Entity;

namespace luchito_net.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(IEnumerable<Category> Categories, int Total)> GetAllCategories(string name, int page, int take, bool onlyActive = true);

        Task<IEnumerable<Category>> GetSubcategories(int parentCategoryId, bool onlyActive = true);

        Task<IEnumerable<Category>> GetCategoryFather();

        Task<Category> CreateCategory(Category category);

        Task<Category> UpdateCategory(int id, Category category);

        Task<Category> GetCategory(int id);

        Task<Category> DeleteCategory(int id);

    }
}