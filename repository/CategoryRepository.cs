using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using Npgsql;
using luchito_net.Models.Entity;

namespace luchito_net.Repository
{
    public class CategoryRepository(InitializeDatabase _context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                var createdCategory = await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync();
                return createdCategory.Entity;
            }
            catch (PostgresException ex)
            {
                _logger.LogError(ex, "Error in CreateCategory for category with Name {CategoryName}", category.Name);
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Category> DeleteCategory(int id)
        {
            try
            {

                var categoryToDelete = await _context.Category.FindAsync(id) ?? throw new Exception($"Category with ID {id} not found.");
                categoryToDelete.IsActive = false;
                await _context.SaveChangesAsync();
                return categoryToDelete;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in DeleteCategory for category ID {CategoryId}", id);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.Category.FindAsync(id) ?? throw new Exception($"Category with ID {id} not found.");
        }

        public async Task<(IEnumerable<Category> Categories, int Total)> GetAllCategories(string name, int page, int take, bool onlyActive = true, bool onlyRootCategories = true)
        {
            var query = await _context.Category
                .Where(c => c.Name.Contains(name))
                .Where(c => onlyActive && c.IsActive || !onlyActive)
                .Where(c => onlyRootCategories && c.ParentCategoryID == null || !onlyRootCategories)
                .Include(c => c.Subcategories)
                .OrderBy(c => c.Name)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return (query, await _context.Category
                .Where(c => c.Name.Contains(name))
                .Where(c => onlyActive && c.IsActive || !onlyActive)
                .Where(c => onlyRootCategories && c.ParentCategoryID == null || !onlyRootCategories)
                .CountAsync());
        }

        public async Task<IEnumerable<Category>> GetSubcategories(int parentCategoryId, bool onlyActive = true)
        {
            return await _context.Set<Category>()
                .Where(c => c.ParentCategoryID == parentCategoryId)
                .OrderBy(c => c.Name)
                .Include(c => c.Subcategories)
                .ToListAsync();
        }


        public async Task<Category> UpdateCategory(int id, Category category)
        {
            try
            {
                Category existingCategory = await _context.Category.FindAsync(id) ?? throw new Exception($"Category with ID {id} not found.");
                existingCategory.Name = category.Name;
                existingCategory.ParentCategoryID = category.ParentCategoryID;
                existingCategory.IsActive = category.IsActive;
                existingCategory.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existingCategory;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in UpdateCategory for category ID {CategoryId}", id);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}