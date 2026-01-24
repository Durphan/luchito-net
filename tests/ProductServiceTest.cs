using System.Data;
using luchito_net.Config.DataProvider;
using luchito_net.Config.DataProvider.Interfaces;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Entity;
using luchito_net.Repository;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using luchito_net.Service.Interfaces;
using luchito_net.Tests.Factory;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace luchito_net.Tests;

public class ProductServiceTest : IDisposable
{
    private readonly InitializeDatabase _context;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryService _categoryService;
    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _context = MemoryDatabaseFactory.CreateInMemoryDatabaseOptions();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var configurationMock = new Mock<IConfiguration>();
        var dapperWrapperMock = new Mock<IDapperWrapper>();
        _productRepository = new ProductRepository(_context, NullLogger<ProductRepository>.Instance, configurationMock.Object, dapperWrapperMock.Object);
        var categoryRepository = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);
        _categoryService = new CategoryService(categoryRepository);
        _productService = new ProductService(_productRepository, _categoryService);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task SearchProductsByName_ReturnsCorrectResponse()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Test Product", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        string query = "test";
        int page = 1;
        int pageSize = 10;
        bool onlyActive = true;


        var result = await _productService.SearchProductsByName(query, page, pageSize, onlyActive);


        Assert.Equal("Test", result.Search);
        Assert.Equal(1, result.Total);
        Assert.Equal(page, result.Page);
        Assert.Equal(pageSize, result.Limit);
        Assert.Single(result.Data);
        var resultProduct = result.Data.First();
        Assert.Equal(productResponse.Id, resultProduct.Id);
        Assert.Equal("Test product", resultProduct.Name);
        Assert.Equal("Category1", resultProduct.Category);
        Assert.True(resultProduct.IsActive);
    }


    [Fact]
    public async Task GetProductById_ReturnsProductResponse()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);


        var result = await _productService.GetProductById(productResponse.Id);

        Assert.Equal(productResponse.Id, result.Id);
        Assert.Equal("Product1", result.Name);
        Assert.Equal("Category1", result.Category);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateProduct_NormalizesNameAndReturnsResponse()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "  test product  ", CategoryId = categoryResponse.Id };


        var result = await _productService.CreateProduct(productDto);


        Assert.Equal(1, result.Id);
        Assert.Equal("Test product", result.Name);
        Assert.Equal("Category1", result.Category);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task UpdateProduct_NormalizesNameAndReturnsResponse()
    {
        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var createProductDto = new ProductRequestDto { Name = "Old product", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(createProductDto);

        var updateProductDto = new ProductRequestDto { Name = "  updated product  ", CategoryId = categoryResponse.Id };


        var result = await _productService.UpdateProduct(productResponse.Id, updateProductDto);

        Assert.Equal(productResponse.Id, result.Id);
        Assert.Equal("Updated product", result.Name);
        Assert.Equal("Category1", result.Category);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsDeletedProductResponse()
    {
        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var result = await _productService.DeleteProduct(productResponse.Id);

        Assert.Equal(productResponse.Id, result.Id);
        Assert.Equal("Product1", result.Name);
        Assert.Equal("Category1", result.Category);
        Assert.False(result.IsActive);
    }
}