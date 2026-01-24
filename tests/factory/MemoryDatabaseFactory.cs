using luchito_net.Config.DataProvider;
using Microsoft.EntityFrameworkCore;

namespace luchito_net.Tests.Factory;

public static class MemoryDatabaseFactory
{
    public static InitializeDatabase CreateInMemoryDatabaseOptions()
    {
        var database = new DbContextOptionsBuilder<InitializeDatabase>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        return new InitializeDatabase(database);


    }
}