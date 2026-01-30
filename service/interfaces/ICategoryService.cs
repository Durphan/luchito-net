using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Entity;


namespace luchito_net.Service.Interfaces
{

    public interface ICategoryService
    {
        Task<CategoriesPaginatedResponseDto> GetAllCategories(string name, int page, int take, bool onlyActive);

        Task<List<CategoryResponseDto>> GetSubcategories(int parentCategoryId, bool onlyActive);

        Task<CategoryResponseDto> CreateCategory(CategoryRequestDto categoryDto);

        Task<CategoriesPaginatedResponseDto> GetParentCategories(int page, int take);

        Task<CategoryResponseDto> GetCategory(int id);

        Task<CategoryResponseDto> UpdateCategory(int id, CategoryRequestDto categoryDto);

        Task<CategoryResponseDto> DeleteCategory(int id);



    }
}