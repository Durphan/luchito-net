using Microsoft.AspNetCore.Mvc;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;

namespace luchito_net.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController(IProviderService providerService) : ControllerBase
    {
        private readonly IProviderService _providerService = providerService;

        [HttpGet]
        public async Task<ActionResult<GetProvidersResponseDto>> GetAllProviders(
            [FromQuery] string? name,
            [FromQuery] bool? isDistributor,
            [FromQuery] int page = 1,
            [FromQuery] int take = 10,
            [FromQuery] bool onlyActive = true)
        {
            var result = await _providerService.GetAllProviders(name ?? string.Empty, isDistributor, page, take, onlyActive);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProviderResponseDto>> CreateProvider([FromBody] ProviderRequestDto providerDto)
        {
            var result = await _providerService.CreateProvider(providerDto);
            return CreatedAtAction(nameof(GetProviderById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderResponseDto>> GetProviderById(int id)
        {
            var result = await _providerService.GetProviderById(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProviderResponseDto>> UpdateProvider(int id, [FromBody] ProviderRequestDto providerDto)
        {
            var result = await _providerService.UpdateProvider(id, providerDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProviderResponseDto>> DeleteProvider(int id)
        {
            var result = await _providerService.DeleteProvider(id);
            return Ok(result);
        }
    }
}