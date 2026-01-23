using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Models.Entity;

namespace luchito_net.Models.Mappers;

public static class ProviderMapper
{
    public static ProviderResponseDto ToResponseDto(this Provider provider)
    {
        return new ProviderResponseDto
        {
            Id = provider.Id,
            Name = provider.Name,
            IsDistributor = provider.IsDistributor,
            IsActive = provider.IsActive
        };
    }

    public static Provider ToEntity(this ProviderRequestDto providerDto)
    {
        return new Provider
        {
            Name = providerDto.Name,
            IsDistributor = providerDto.IsDistributor,
            IsActive = providerDto.IsActive ?? true
        };
    }

    public static GetProvidersResponseDto ToGetProvidersResponseDto(this IEnumerable<Provider> providers, int total, int page, int take, string search, bool? isDistributor)
    {
        return new GetProvidersResponseDto
        {
            Search = search,
            StateFiltered = isDistributor?.ToString() ?? string.Empty,
            Total = total,
            Page = page,
            Limit = take,
            Data = providers.Select(p => p.ToResponseDto())
        };
    }
}