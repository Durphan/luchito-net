
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using luchito_net.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace luchito_net.View.Pages;


public class ProductModel(ILogger<ProductModel> logger, IProductService productService, ICategoryService categoryService, IOrderService orderService) : PageModel
{


    private readonly ILogger<ProductModel> _logger = logger;

    private readonly IProductService _productService = productService;

    private readonly ICategoryService _categoryService = categoryService;

    private readonly IOrderService _orderService = orderService;

    public GetAllCategoriesResponseDto? _categories;

    public ProductSearchCategoryResponseDto? _products;

    public ProductSearchResponseDto? _allProducts;



    public async Task<IActionResult> OnGetAsync(int? categoryId = null, int categoryPage = 1, int productPage = 1)
    {
        _categories = await _categoryService.GetAllCategories("", categoryPage, 10, true, true);
        if (categoryId.HasValue)
        {
            _products = await _productService.SearchProductsByCategory(categoryId.Value, productPage, 10, true);
        }
        else
        {
            _allProducts = await _productService.SearchProductsByName("", productPage, 10, true);
        }
        return Page();
    }



    public async Task<IActionResult> OnCategoryEdit(CategoryRequestDto category, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _categoryService.UpdateCategory(id, category);
        return Page();
    }


    public async Task<IActionResult> OnPostUpdateCategoryAsync(CategoryRequestDto category, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _categoryService.UpdateCategory(id, category);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteCategoryAsync(int id)
    {
        Console.WriteLine($"Deleting category with ID: {id}");
        await _categoryService.DeleteCategory(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateOrderAsync(OrderRequestDto order, string boxed)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        order.IsBoxed = boxed == "on";
        await _orderService.CreateOrder(order);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateCategoryAsync(CategoryRequestDto category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _categoryService.CreateCategory(category);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateProductAsync(ProductRequestDto product, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _productService.UpdateProduct(id, product);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCreateProductAsync(ProductRequestDto product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _productService.AddProduct(product);
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostDeleteProductAsync(int id)
    {
        Console.WriteLine($"Deleting product with ID: {id}");
        await _productService.DeleteProduct(id);
        return RedirectToPage();
    }


    public async Task<IActionResult> OnPostUpdateOrderAsync(OrderRequestDto order, int id, string boxed)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        order.IsBoxed = boxed == "on";
        await _orderService.UpdateOrder(id, order);
        return RedirectToPage();
    }
}

