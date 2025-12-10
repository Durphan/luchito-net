using System.Diagnostics;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace luchito_net.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ProductModel(ILogger<ProductModel> logger) : PageModel
{


    private readonly ILogger<ProductModel> _logger = logger;

    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;
    public void OnGet()
    {
    }

    public ProductSearchResponseDto GetProducts(string query, int page = 1, int pageSize = 10, bool onlyActive = true)
    {
        var result = _productService.SearchProductsByName(query, page, pageSize, onlyActive).Result;
        return result;
    }

}

