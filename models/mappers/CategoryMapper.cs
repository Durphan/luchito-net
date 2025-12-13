using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;

namespace luchito_net.Models.Mappers;

public static class CategoryMapper
{
    public static CategoryResponseDto ToResponseDto(this Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryID = category.ParentCategoryID,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
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

    public static GetAllCategoriesResponseDto ToGetAllCategoriesResponseDto(this IEnumerable<Category> categories, int total, int page, int take)
    {
        return new GetAllCategoriesResponseDto
        {

            Data = categories.Select(c => c.ToResponseDto()),
            Total = total,
            Page = page,
            Limit = take
        };
    }


    public static CategoryWithSubcategoriesAndProductsResponseDto ToCategoryWithSubcategoriesAndProductsResponseDto(this Category category, Dictionary<int, Category> categoryDict)
    {
        CategoryWithSubcategoriesAndProductsResponseDto dto = new()
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryID = category.ParentCategoryID,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            Subcategories = [.. categoryDict.Values
                    .Where(c => c.ParentCategoryID == category.Id)
                    .Select(c => ToCategoryWithSubcategoriesAndProductsResponseDto(c, categoryDict))],
            Products = category.Products?.Select(p => p.ToResponseDto()).ToList() ?? []
        };
        return dto;
    }
}