using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;


namespace luchito_net.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<GetAllCategoriesResponseDto> GetAllCategories(string name, int page, int take, bool onlyActive, bool onlyRootCategories);

        Task<List<CategoryResponseDto>> GetSubcategories(int parentCategoryId, bool onlyActive);

        Task<CategoryResponseDto> CreateCategory(CategoryRequestDto categoryDto);

        Task<CategoryResponseDto> GetCategoryById(int id);

        Task<CategoryResponseDto> UpdateCategory(int id, CategoryRequestDto categoryDto);

        Task<CategoryResponseDto> DeleteCategory(int id);



    }
}