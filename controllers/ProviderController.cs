using Microsoft.AspNetCore.Mvc;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Dto.Response;
using luchito_net.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace luchito_net.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProviderController(IProviderService providerService) : ControllerBase
    {
        private readonly IProviderService _providerService = providerService;

        /// <summary>
        /// Get all providers with optional filters and pagination
        /// </summary>
        /// <param name="name">Filter providers by name</param>
        /// <param name="isDistributor">Filter providers by distributor status</param>
        /// <param name="page">Page number for pagination</param>
        /// <param name="take">Number of providers to take per page</param>
        /// <param name="onlyActive">Filter to include only active providers</param>
        /// <returns>List of providers matching the filters</returns>
        [HttpGet("getProviders")]
        [SwaggerOperation(Summary = "Get all providers with optional filters and pagination")]
        [ProducesResponseType(typeof(GetProvidersResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
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

        /// <summary>
        /// Create a new provider
        /// </summary>
        /// <param name="providerDto">Provider data transfer object containing the details of the provider to create</param>
        /// <returns>Details of the newly created provider  </returns>
        [HttpPost("createProvider")]
        [SwaggerOperation(Summary = "Create a new provider")]
        [ProducesResponseType(typeof(ProviderResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProviderResponseDto>> CreateProvider([FromBody] ProviderRequestDto providerDto)
        {
            var result = await _providerService.CreateProvider(providerDto);
            return CreatedAtAction(nameof(GetProviderById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Get provider by ID
        /// </summary>
        /// <param name="id">The ID of the provider to retrieve</param>
        /// <returns>Details of the provider with the specified ID</returns>
        [HttpGet("provider/{id}")]
        [SwaggerOperation(Summary = "Get provider by ID")]
        [ProducesResponseType(typeof(ProviderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProviderResponseDto>> GetProviderById(int id)
        {
            var result = await _providerService.GetProviderById(id);
            return Ok(result);
        }

        /// <summary>
        /// Update an existing provider
        /// </summary>
        /// <param name="id">The ID of the provider to update</param>
        /// <param name="providerDto">Provider data transfer object containing the updated details of the provider</param>
        /// <returns>Details of the updated provider</returns>
        [HttpPut("updateProvider/{id}")]
        [SwaggerOperation(Summary = "Update an existing provider")]
        [ProducesResponseType(typeof(ProviderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProviderResponseDto>> UpdateProvider(int id, [FromBody] ProviderRequestDto providerDto)
        {
            var result = await _providerService.UpdateProvider(id, providerDto);
            return Ok(result);
        }
        /// <summary>
        /// Delete a provider by ID
        /// </summary>
        /// <param name="id">The ID of the provider to delete</param>
        /// <returns>Details of the deleted provider</returns>
        [HttpDelete("deleteProvider/{id}")]
        [SwaggerOperation(Summary = "Delete a provider by ID")]
        [ProducesResponseType(typeof(ProviderResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public async Task<ActionResult<ProviderResponseDto>> DeleteProvider(int id)
        {
            var result = await _providerService.DeleteProvider(id);
            return Ok(result);
        }
    }
}