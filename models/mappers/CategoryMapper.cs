using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Entity;

namespace luchito_net.Models.Mappers;

public static class CategoryMapper
{
    public static CategoryResponseDto ToResponseDto(this Category category)
    {
        return new CategoryResponseDto(category);
    }

    public static Category ToEntity(this CategoryRequestDto categoryDto)
    {
        return new Category
        {
            Name = categoryDto.Name,
            ParentCategoryID = categoryDto.ParentCategoryID,
            IsActive = categoryDto.IsActive ?? true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CategoriesPaginatedResponseDto ToGetAllCategoriesResponseDto(this IEnumerable<Category> categories, int total, int page, int take)
    {
        return new CategoriesPaginatedResponseDto
        {

            Data = categories.Select(c => c.ToResponseDto()),
            Total = total,
            Page = page,
            Limit = take
        };
    }



}