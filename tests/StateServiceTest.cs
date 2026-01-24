using luchito_net.Config.DataProvider;
using luchito_net.Models.Entity;
using luchito_net.Repository;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using luchito_net.Tests.Factory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace luchito_net.Tests;

public class StateServiceTest : IDisposable
{
    private readonly InitializeDatabase _context;
    private readonly IStateRepository _stateRepository;
    private readonly StateService _stateService;

    public StateServiceTest()
    {

        _context = MemoryDatabaseFactory.CreateInMemoryDatabaseOptions();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _stateRepository = new StateRepository(_context, NullLogger<StateRepository>.Instance);

        _stateService = new StateService(NullLogger<StateService>.Instance, _stateRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task GetStateById_ReturnsState()
    {
        // Arrange
        int id = 1;

        // Act
        var result = await _stateService.GetStateById(id);

        // Assert
        Assert.Equal(id, result.Id);
        Assert.Equal("Pendiente", result.Name); // From seeded data
    }
}