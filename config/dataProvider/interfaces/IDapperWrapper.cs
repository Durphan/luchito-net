using System.Data;

namespace luchito_net.Config.DataProvider.Interfaces
{
    public interface IDapperWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(IConfiguration configuration, string sql, object? param = null, CommandType? commandType = null);
    }
}