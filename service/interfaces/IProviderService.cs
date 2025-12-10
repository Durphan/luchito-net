using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;

namespace luchito_net.Service.Interfaces
{
    public interface IProviderService
    {
        Task<GetProvidersResponseDto> GetAllProviders(string name, bool? isDistributor, int page, int take, bool onlyActive = true);

        Task<ProviderResponseDto> CreateProvider(ProviderRequestDto providerDto);

        Task<ProviderResponseDto> GetProviderById(int id);

        Task<ProviderResponseDto> UpdateProvider(int id, ProviderRequestDto providerDto);

        Task<ProviderResponseDto> DeleteProvider(int id);
    }
}