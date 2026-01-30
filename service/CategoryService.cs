using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Entity;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;
using luchito_net.Utils;

namespace luchito_net.Service
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository = categoryRepository;


        async public Task<CategoryResponseDto> CreateCategory(CategoryRequestDto categoryDto)
        {
            categoryDto.Name = NameNormalizer.Normalize(categoryDto.Name);
            var createdCategory = await _categoryRepository.CreateCategory(categoryDto.ToEntity());
            return createdCategory.ToResponseDto();
        }

        async public Task<CategoryResponseDto> DeleteCategory(int id)
        {
            var deletedCategory = await _categoryRepository.DeleteCategory(id);
            return deletedCategory.ToResponseDto();
        }

        async public Task<CategoriesPaginatedResponseDto> GetAllCategories(string name, int page, int take, bool onlyActive, bool onlyRootCategories)
        {
            name = NameNormalizer.NormalizeSearch(name);
            (IEnumerable<Category>, int) categories = await _categoryRepository.GetAllCategories(name, page, take, onlyActive, onlyRootCategories);
            return categories.Item1.ToGetAllCategoriesResponseDto(categories.Item2, page, take);
        }

        public async Task<CategoryResponseDto> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategory(id);
            return category.ToResponseDto();
        }

        public async Task<CategoriesPaginatedResponseDto> GetParentCategories(int page, int take)
        {
            var categories = await _categoryRepository.GetCategoryFather();
            return categories.ToGetAllCategoriesResponseDto(categories.Count(), page, take);
        }

        public async Task<CategoryResponseDto> UpdateCategory(int id, CategoryRequestDto categoryDto)
        {
            categoryDto.Name = NameNormalizer.Normalize(categoryDto.Name);
            var category = await _categoryRepository.UpdateCategory(id, categoryDto.ToEntity());
            return category.ToResponseDto();
        }

        public async Task<List<CategoryResponseDto>> GetSubcategories(int parentCategoryId, bool onlyActive)
        {
            var subcategories = await _categoryRepository.GetSubcategories(parentCategoryId, onlyActive);
            return [.. subcategories.Select(c => c.ToResponseDto())];
        }

    }
}