using luchito_net.Models;

namespace luchito_net.Service.Interfaces
{
    public interface IStateService
    {
        Task<State> GetStateById(int id);

        Task<IEnumerable<State>> GetAllStates();
    }
}