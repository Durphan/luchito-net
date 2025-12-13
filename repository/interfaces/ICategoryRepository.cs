using luchito_net.Models;

namespace luchito_net.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<(IEnumerable<Category> Categories, int Total)> GetAllCategories(string name, int page, int take, bool onlyActive = true);

        Task<Category> CreateCategory(Category category);

        Task<Category> UpdateCategory(int id, Category category);

        Task<Category> GetCategoryById(int id);

        Task<Category> DeleteCategory(int id);

        Task<IEnumerable<Category>> GetAllCategoriesWithHierarchy(bool onlyActive = true);
    }
}