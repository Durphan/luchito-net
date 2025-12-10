using Microsoft.EntityFrameworkCore;
using luchito_net.Config.DataProvider;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;

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
                Provider createdProvider = (await _context.AddAsync(provider)).Entity;
                await _context.SaveChangesAsync();
                return createdProvider;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateProvider for provider {ProviderName}", provider.Name);
                throw;
            }
        }

        public async Task<Provider> DeleteProvider(int id)
        {
            try
            {
                Provider providerToDelete = (await _context.Set<Provider>().FindAsync(id)) ?? throw new Exception("Provider with ID " + id + " not found.");
                providerToDelete.IsActive = false;
                _context.Set<Provider>().Update(providerToDelete);
                await _context.SaveChangesAsync();
                return providerToDelete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteProvider for provider ID {ProviderId}", id);
                throw;
            }
        }

        public async Task<Provider> GetProviderById(int id)
        {
            try
            {
                return await _context.Set<Provider>().FindAsync(id) ?? throw new Exception("Provider with ID " + id + " not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProviderById for provider ID {ProviderId}", id);
                throw;
            }
        }

        public async Task<(IEnumerable<Provider> Providers, int Total)> GetAllProviders(string name, bool? isDistributor, int page, int take, bool onlyActive = true)
        {
            try
            {
                IQueryable<Provider> query = _context.Set<Provider>()
                    .Where(p => p.Name.Contains(name))
                    .OrderBy(p => p.Name);
                if (isDistributor.HasValue)
                {
                    query = query.Where(p => p.IsDistributor == isDistributor.Value);
                }
                if (onlyActive)
                {
                    query = query.Where(p => p.IsActive);
                }
                IEnumerable<Provider> providers = await query
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync();
                int total = await query.CountAsync();
                return (providers, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllProviders with name {Name}, isDistributor {IsDistributor}, page {Page}, take {Take}, onlyActive {OnlyActive}", name, isDistributor, page, take, onlyActive);
                throw;
            }
        }

        public async Task<Provider> UpdateProvider(int id, Provider provider)
        {
            try
            {
                Provider existingProvider = (await _context.Set<Provider>().FindAsync(id)) ?? throw new Exception("Provider with ID " + id + " not found.");
                existingProvider.Name = provider.Name;
                existingProvider.IsDistributor = provider.IsDistributor;
                existingProvider.IsActive = provider.IsActive;
                _context.Set<Provider>().Update(existingProvider);
                await _context.SaveChangesAsync();
                return existingProvider;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateProvider for provider ID {ProviderId}", id);
                throw;
            }
        }
    }
}