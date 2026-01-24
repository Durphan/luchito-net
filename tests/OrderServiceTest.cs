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

public class OrderServiceTest : IDisposable
{
    private readonly InitializeDatabase _context;
    private readonly IOrderRepository _orderRepository;
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IProviderService _providerService;
    private readonly OrderService _orderService;

    public OrderServiceTest()
    {
        _context = MemoryDatabaseFactory.CreateInMemoryDatabaseOptions();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _orderRepository = new OrderRepository(_context, NullLogger<OrderRepository>.Instance);

        var categoryRepository = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);
        _categoryService = new CategoryService(categoryRepository);

        var configurationMock = new Mock<IConfiguration>();
        var dapperWrapperMock = new Mock<IDapperWrapper>();
        var productRepository = new ProductRepository(_context, NullLogger<ProductRepository>.Instance, configurationMock.Object, dapperWrapperMock.Object);
        _productService = new ProductService(productRepository, _categoryService);

        var providerRepository = new ProviderRepository(_context, NullLogger<ProviderRepository>.Instance);
        _providerService = new ProviderService(NullLogger<ProviderService>.Instance, providerRepository);

        _orderService = new OrderService(_orderRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetAllOrders_ReturnsCorrectResponse()
    {
        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        var orderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = true,
            StateId = 1,
            ProviderId = providerResponse.Id,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(orderDto);

        int page = 1;
        int take = 10;
        bool onlyActive = true;

        var result = await _orderService.GetAllOrders(page, take, onlyActive);

        Assert.Equal(1, result.Total);
        Assert.Equal(page, result.Page);
        Assert.Equal(take, result.Limit);
        Assert.Single(result.Data);
        var resultOrder = result.Data.First();
        Assert.Equal(orderResponse.Id, resultOrder.Id);
        Assert.Equal("Product1", resultOrder.Product);
        Assert.Equal(10, resultOrder.Quantity);
        Assert.Equal("Pendiente", resultOrder.State);
        Assert.Equal("Provider1", resultOrder.Provider);
        Assert.True(resultOrder.IsBoxed);
        Assert.True(resultOrder.IsActive);
    }

    [Fact]
    public async Task CreateOrder_ReturnsOrderResponse()
    {
        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var orderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 5,
            StateId = 1,
            IsBoxed = false,
            ProviderId = null,
            IsActive = true
        };

        var result = await _orderService.CreateOrder(orderDto);

        Assert.Equal(1, result.Id);
        Assert.Equal("Product1", result.Product);
        Assert.Equal(5, result.Quantity);
        Assert.Equal("Pendiente", result.State);
        Assert.Equal("Sin Proveedor", result.Provider);
        Assert.False(result.IsBoxed);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetOrderById_ReturnsOrderResponse()
    {
        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        var orderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = true,
            StateId = 1,
            ProviderId = providerResponse.Id,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(orderDto);

        var result = await _orderService.GetOrderById(orderResponse.Id);


        Assert.Equal(orderResponse.Id, result.Id);
        Assert.Equal("Product1", result.Product);
        Assert.Equal(10, result.Quantity);
        Assert.Equal("Pendiente", result.State);
        Assert.Equal("Provider1", result.Provider);
        Assert.True(result.IsBoxed);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task UpdateOrder_ValidUpdate_ReturnsOrderResponse()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        var createOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = false,
            StateId = 1,
            ProviderId = null,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(createOrderDto);

        var updateOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 20,
            StateId = 2,
            IsBoxed = true,
            ProviderId = providerResponse.Id,
            IsActive = true
        };


        var result = await _orderService.UpdateOrder(orderResponse.Id, updateOrderDto);


        Assert.Equal(orderResponse.Id, result.Id);
        Assert.Equal("Product1", result.Product);
        Assert.Equal(20, result.Quantity);
        Assert.Equal("Comprado", result.State); // From seeded
        Assert.Equal("Provider1", result.Provider);
        Assert.True(result.IsBoxed);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task UpdateOrder_StateId2WithoutProviderId_ThrowsException()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var createOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = false,
            StateId = 1,
            ProviderId = null,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(createOrderDto);

        var updateOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 20,
            StateId = 2,
            IsBoxed = true,
            ProviderId = null,
            IsActive = true
        };


        await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateOrder(orderResponse.Id, updateOrderDto));
    }

    [Fact]
    public async Task UpdateOrder_StateId3WithoutProviderId_ThrowsException()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var createOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = false,
            StateId = 1,
            ProviderId = null,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(createOrderDto);

        var updateOrderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 20,
            StateId = 3,
            IsBoxed = true,
            ProviderId = null,
            IsActive = true
        };


        await Assert.ThrowsAsync<Exception>(() => _orderService.UpdateOrder(orderResponse.Id, updateOrderDto));
    }

    [Fact]
    public async Task DeleteOrder_ReturnsDeletedOrderResponse()
    {

        var categoryDto = new CategoryRequestDto { Name = "Category1", IsActive = true, ParentCategoryID = null };
        var categoryResponse = await _categoryService.CreateCategory(categoryDto);

        var productDto = new ProductRequestDto { Name = "Product1", CategoryId = categoryResponse.Id };
        var productResponse = await _productService.CreateProduct(productDto);

        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        var orderDto = new OrderRequestDto
        {
            ProductId = productResponse.Id,
            Quantity = 10,
            IsBoxed = true,
            StateId = 1,
            ProviderId = providerResponse.Id,
            IsActive = true
        };
        var orderResponse = await _orderService.CreateOrder(orderDto);


        var result = await _orderService.DeleteOrder(orderResponse.Id);


        Assert.Equal(orderResponse.Id, result.Id);
        Assert.Equal("Product1", result.Product);
        Assert.Equal(10, result.Quantity);
        Assert.Equal("Pendiente", result.State);
        Assert.Equal("Provider1", result.Provider);
        Assert.True(result.IsBoxed);
        Assert.False(result.IsActive);
    }
}