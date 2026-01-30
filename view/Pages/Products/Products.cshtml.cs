
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using luchito_net.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using luchito_net.Models.Entity;

namespace luchito_net.View.Pages.Products;


public class ProductModel(ILogger<ProductModel> logger, IProductService productService, ICategoryService categoryService, IOrderService orderService) : PageModel
{


    private readonly ILogger<ProductModel> _logger = logger;

    private readonly IProductService _productService = productService;

    private readonly ICategoryService _categoryService = categoryService;

    private readonly IOrderService _orderService = orderService;

    public CategoriesPaginatedResponseDto? _categories;

    public ProductSearchCategoryResponseDto? _products;

    public ProductSearchResponseDto? _allProducts;

    public int? IdCategorySelected = null;

    public int CurrentCategoryPage = 1;


    public async Task<IActionResult> OnGetAsync(int? categoryId = null, int categoryPage = 1, int productPage = 1)
    {
        _categories = await _categoryService.GetParentCategories(categoryPage, 10);
        if (categoryId.HasValue)
        {
            _products = await _productService.SearchProductsByCategory(categoryId.Value, productPage, 10, true);
            IdCategorySelected = categoryId.Value;
            return Page();
        }
        _allProducts = await _productService.SearchProductsByName("", productPage, 10, true);
        IdCategorySelected = null;
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

    public async Task OnPostDeleteCategoryAsync(int id, int page)
    {
        await _categoryService.DeleteCategory(id);
        IdCategorySelected = null;
    }

    public async Task SelectCategoryAsync(int id)
    {
        IdCategorySelected = id;
        _products = await _productService.SearchProductsByCategory(id, 1, 10, true);
    }


    public async Task<IActionResult> OnPostCreateOrderAsync(OrderRequestDto order, string boxed)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        order.IsBoxed = boxed == "on";
        await _orderService.CreateOrder(order);
        return new JsonResult(new { success = true });
    }

    public async Task<List<CategoryResponseDto>> GetSubcategories(int parentId)
    {
        var categories = await _categoryService.GetSubcategories(parentId, true);
        return categories;
    }

    public async Task OnPostCreateCategoryAsync(CategoryRequestDto category)
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        await _categoryService.CreateCategory(category);
        _categories = await _categoryService.GetParentCategories(CurrentCategoryPage, 10);

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
        await _productService.CreateProduct(product);
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

