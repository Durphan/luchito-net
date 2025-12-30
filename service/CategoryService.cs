using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;

namespace luchito_net.Service
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        async public Task<CategoryResponseDto> CreateCategory(CategoryRequestDto categoryDto)
        {
            Category createdCategory = await _categoryRepository.CreateCategory(categoryDto.ToEntity());
            return createdCategory.ToResponseDto();
        }

        async public Task<CategoryResponseDto> DeleteCategory(int id)
        {
            Category deletedCategory = await _categoryRepository.DeleteCategory(id);
            return deletedCategory.ToResponseDto();
        }

        async public Task<GetAllCategoriesResponseDto> GetAllCategories(string name, int page, int take, bool onlyActive)
        {
            (IEnumerable<Category>, int) categories = await _categoryRepository.GetAllCategories(name, page, take, onlyActive);
            return categories.Item1.ToGetAllCategoriesResponseDto(categories.Item2, page, take);
        }

        public async Task<CategoryResponseDto> GetCategoryById(int id)
        {
            Category category = await _categoryRepository.GetCategoryById(id);
            return category.ToResponseDto();
        }

        public async Task<CategoryResponseDto> UpdateCategory(int id, CategoryRequestDto categoryDto)
        {
            Category category = await _categoryRepository.UpdateCategory(id, categoryDto.ToEntity());
            return category.ToResponseDto();
        }

        public async Task<List<CategoryWithSubcategoriesAndProductsResponseDto>> GetAllCategoriesWithHierarchy(bool onlyActive = true)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesWithHierarchy(onlyActive);
            Dictionary<int, Category> categoryDict = categories.ToDictionary(c => c.Id, c => c);
            List<CategoryWithSubcategoriesAndProductsResponseDto> categoryHierarchy = [];
            foreach (Category category in categoryDict.Values.Where(c => c.ParentCategoryID == null))
            {
                categoryHierarchy.Add(category.ToCategoryWithSubcategoriesAndProductsResponseDto(categoryDict));
            }
            return categoryHierarchy;
        }

    }
}