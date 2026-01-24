using System.Data;
using Dapper;

namespace luchito_net.Config.DataProvider;

public class DapperWrapper : Interfaces.IDapperWrapper
{

    public async Task<IEnumerable<T>> QueryAsync<T>(IConfiguration configuration, string sql, object? param = null, CommandType? commandType = null)
    {
        using IDbConnection connection = new Npgsql.NpgsqlConnection(configuration.GetConnectionString("DockerPostgreSql"));
        return await connection.QueryAsync<T>(sql, param, commandType: commandType);
    }
}