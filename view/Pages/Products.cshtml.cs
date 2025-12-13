using luchito_net.Models;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace luchito_net.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ProductModel(ILogger<ProductModel> logger, IProductService productService, ICategoryService categoryService) : PageModel
{


    private readonly ILogger<ProductModel> _logger = logger;

    private readonly IProductService _productService = productService;

    private readonly ICategoryService _categoryService = categoryService;

    public ProductSearchResponseDto? _products;

    public List<CategoryResponseDto>? _categories;

    public async Task OnGet()
    {
        _products = SearchProductsByName().Result;
        _categories = await _categoryService.GetAllCategoriesWithHierarchy(true);
    }

    public async Task<ProductSearchCategoryResponseDto> GetProductsByCategoryId(int categoryId, int pageNumber = 1)
    {
        return await _productService.SearchProductsByCategory(categoryId, pageNumber, 10, true);
    }

    public async Task<GetAllCategoriesResponseDto> GetCategoriesByName(string name = "", int pageNumber = 1)
    {
        return await _categoryService.GetAllCategories(name, pageNumber, 10, true);
    }

    public async Task<ProductSearchResponseDto> SearchProductsByName(string name = "", int pageNumber = 1)
    {
        return await _productService.SearchProductsByName(name, pageNumber, 10, true);
    }





}

