using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Errors;

namespace luchito_net.Repository
{
    public class CategoryRepository(InitializeDatabase _context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<Category> CreateCategory(Category category)
        {
            Category createdCategory = (await _context.AddAsync(category)).Entity;
            await _context.SaveChangesAsync();
            return createdCategory;
        }

        public async Task<Category> DeleteCategory(int id)
        {

            Category categoryToDelete = (await _context.Set<Category>().FindAsync(id)) ?? throw new NotFoundException($"Category with ID {id} not found.");
            categoryToDelete.IsActive = false;
            _context.Set<Category>().Update(categoryToDelete);
            await _context.SaveChangesAsync();
            return categoryToDelete;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Set<Category>().FindAsync(id) ?? throw new NotFoundException($"Category with ID {id} not found.");

        }

        public async Task<(IEnumerable<Category> Categories, int Total)> GetAllCategories(string name, int page, int take, bool onlyActive = true)
        {
            IQueryable<Category> query = _context.Set<Category>()
                .Where(c => c.Name.Contains(name))
                .OrderBy(c => c.Name);
            if (onlyActive)
            {
                query = query.Where(c => c.IsActive);
            }
            IEnumerable<Category> categories = await query
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            int total = await query.CountAsync();
            return (categories, total);

        }

        public async Task<Category> UpdateCategory(int id, Category category)
        {
            Category existingCategory = (await _context.Set<Category>().FindAsync(id)) ?? throw new Exception($"Category with ID {id} not found.");
            existingCategory.Name = category.Name;
            existingCategory.ParentCategoryID = category.ParentCategoryID;
            existingCategory.IsActive = category.IsActive;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            _context.Set<Category>().Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;

        }
    }
}