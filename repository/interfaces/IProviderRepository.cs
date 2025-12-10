using luchito_net.Models;

namespace luchito_net.Repository.Interfaces
{
    public interface IProviderRepository
    {
        Task<(IEnumerable<Provider> Providers, int Total)> GetAllProviders(string name, bool? isDistributor, int page, int take, bool onlyActive = true);

        Task<Provider> CreateProvider(Provider provider);

        Task<Provider> UpdateProvider(int id, Provider provider);

        Task<Provider> GetProviderById(int id);

        Task<Provider> DeleteProvider(int id);
    }
}