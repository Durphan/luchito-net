using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Entity;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;
using luchito_net.Utils;

namespace luchito_net.Service
{
    public class ProviderService(ILogger<ProviderService> logger, IProviderRepository providerRepository) : IProviderService
    {

        private readonly ILogger<ProviderService> _logger = logger;
        private readonly IProviderRepository _providerRepository = providerRepository;

        public async Task<GetProvidersResponseDto> GetAllProviders(string name, bool? isDistributor, int page, int take, bool onlyActive = true)
        {
            (IEnumerable<Provider> providers, int total) = await _providerRepository.GetAllProviders(name, isDistributor, page, take, onlyActive);
            return providers.ToGetProvidersResponseDto(total, page, take, name, isDistributor);
        }

        public async Task<ProviderResponseDto> CreateProvider(ProviderRequestDto providerDto)
        {
            providerDto.Name = NameNormalizer.Normalize(providerDto.Name);
            Provider createdProvider = await _providerRepository.CreateProvider(providerDto.ToEntity());
            return createdProvider.ToResponseDto();
        }

        public async Task<ProviderResponseDto> GetProviderById(int id)
        {
            Provider provider = await _providerRepository.GetProviderById(id);
            return provider.ToResponseDto();
        }

        public async Task<ProviderResponseDto> UpdateProvider(int id, ProviderRequestDto providerDto)
        {
            providerDto.Name = NameNormalizer.Normalize(providerDto.Name);
            Provider provider = await _providerRepository.UpdateProvider(id, providerDto.ToEntity());
            return provider.ToResponseDto();
        }

        public async Task<ProviderResponseDto> DeleteProvider(int id)
        {
            Provider deletedProvider = await _providerRepository.DeleteProvider(id);
            return deletedProvider.ToResponseDto();
        }
    }
}