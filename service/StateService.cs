using luchito_net.Models.Entity;
using luchito_net.Repository.Interfaces;
using luchito_net.Service.Interfaces;

namespace luchito_net.Service
{
    public class StateService(ILogger<StateService> logger, IStateRepository stateRepository) : IStateService
    {

        private readonly ILogger<StateService> _logger = logger;
        private readonly IStateRepository _stateRepository = stateRepository;

        public async Task<State> GetStateById(int id)
        {
            return await _stateRepository.GetStateById(id);
        }

    }
}