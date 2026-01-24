using luchito_net.Config.DataProvider;
using luchito_net.Models.Dto.Request;
using luchito_net.Repository;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using luchito_net.Tests.Factory;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace luchito_net.Tests;

public class ProviderServiceTest : IDisposable
{
    private readonly InitializeDatabase _context;
    private readonly Mock<ILogger<ProviderService>> _loggerMock;
    private readonly IProviderRepository _providerRepository;
    private readonly ProviderService _providerService;

    public ProviderServiceTest()
    {
        _context = MemoryDatabaseFactory.CreateInMemoryDatabaseOptions();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        _providerRepository = new ProviderRepository(_context, NullLogger<ProviderRepository>.Instance);

        _loggerMock = new Mock<ILogger<ProviderService>>();
        _providerService = new ProviderService(_loggerMock.Object, _providerRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetAllProviders_ReturnsCorrectResponse()
    {
        // Seed data
        var providerDto1 = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerDto2 = new ProviderRequestDto { Name = "Provider2", IsDistributor = false, IsActive = true };
        await _providerService.CreateProvider(providerDto1);
        await _providerService.CreateProvider(providerDto2);

        string name = "";
        bool? isDistributor = null;
        int page = 1;
        int take = 10;
        bool onlyActive = true;

        var result = await _providerService.GetAllProviders(name, isDistributor, page, take, onlyActive);

        Assert.Equal(2, result.Total);
        Assert.Equal(page, result.Page);
        Assert.Equal(take, result.Limit);
        Assert.Equal(name, result.Search);
        Assert.True(string.IsNullOrEmpty(result.StateFiltered));
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task CreateProvider_NormalizesNameAndReturnsResponse()
    {
        var providerDto = new ProviderRequestDto { Name = "  test provider  ", IsDistributor = true, IsActive = true };

        var result = await _providerService.CreateProvider(providerDto);

        Assert.Equal("Test provider", result.Name);
        Assert.True(result.IsDistributor);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetProviderById_ReturnsProviderResponse()
    {
        // Arrange
        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        // Act
        var result = await _providerService.GetProviderById(providerResponse.Id);

        // Assert
        Assert.Equal(providerResponse.Id, result.Id);
        Assert.Equal("Provider1", result.Name);
        Assert.True(result.IsDistributor);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task UpdateProvider_NormalizesNameAndReturnsResponse()
    {
        // Arrange
        var createProviderDto = new ProviderRequestDto { Name = "Old provider", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(createProviderDto);

        var updateProviderDto = new ProviderRequestDto { Name = "  updated provider  ", IsDistributor = false, IsActive = true };

        // Act
        var result = await _providerService.UpdateProvider(providerResponse.Id, updateProviderDto);

        // Assert
        Assert.Equal(providerResponse.Id, result.Id);
        Assert.Equal("Updated provider", result.Name);
        Assert.False(result.IsDistributor);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task DeleteProvider_ReturnsDeletedProviderResponse()
    {
        // Arrange
        var providerDto = new ProviderRequestDto { Name = "Provider1", IsDistributor = true, IsActive = true };
        var providerResponse = await _providerService.CreateProvider(providerDto);

        // Act
        var result = await _providerService.DeleteProvider(providerResponse.Id);

        // Assert
        Assert.Equal(providerResponse.Id, result.Id);
        Assert.Equal("Provider1", result.Name);
        Assert.True(result.IsDistributor);
        Assert.False(result.IsActive);
    }
}