using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Models.Entity;

namespace luchito_net.Repository
{
    public class ProviderRepository(InitializeDatabase _context, ILogger<ProviderRepository> logger) : IProviderRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<ProviderRepository> _logger = logger;

        public async Task<Provider> CreateProvider(Provider provider)
        {
            try
            {
                var createdProvider = await _context.Provider.AddAsync(provider);
                await _context.SaveChangesAsync();
                return createdProvider.Entity;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in CreateProvider for provider {ProviderName}", provider.Name);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Provider> DeleteProvider(int id)
        {
            try
            {
                Provider providerToDelete = await _context.Provider.FindAsync(id) ?? throw new Exception("Provider with ID " + id + " not found.");
                providerToDelete.IsActive = false;
                await _context.SaveChangesAsync();
                return providerToDelete;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in DeleteProvider for provider ID {ProviderId}", id);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Provider> GetProviderById(int id)
        {
            return await _context.Provider.FindAsync(id) ?? throw new Exception("Provider with ID " + id + " not found.");
        }

        public async Task<(IEnumerable<Provider> Providers, int Total)> GetAllProviders(string name, bool? isDistributor, int page, int take, bool onlyActive = true)
        {
            var query = await _context.Provider
                .Where(p => p.Name.Contains(name))
                .Where(p => !isDistributor.HasValue || p.IsDistributor == isDistributor.Value)
                .Where(p => !onlyActive || p.IsActive)
                .OrderBy(p => p.Name)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();
            return (query, await _context.Provider
                .Where(p => p.Name.Contains(name))
                .Where(p => !isDistributor.HasValue || p.IsDistributor == isDistributor.Value)
                .Where(p => !onlyActive || p.IsActive)
                .CountAsync());
        }

        public async Task<Provider> UpdateProvider(int id, Provider provider)
        {
            try
            {
                Provider existingProvider = await _context.Provider.FindAsync(id) ?? throw new Exception("Provider with ID " + id + " not found.");
                existingProvider.Name = provider.Name;
                existingProvider.IsDistributor = provider.IsDistributor;
                existingProvider.IsActive = provider.IsActive;
                await _context.SaveChangesAsync();
                return existingProvider;
            }
            catch (Npgsql.PostgresException ex)
            {
                _logger.LogError(ex, "Error in UpdateProvider for provider ID {ProviderId}", id);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}