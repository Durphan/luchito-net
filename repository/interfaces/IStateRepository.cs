using luchito_net.Models;

namespace luchito_net.Repository.Interfaces
{
    public interface IStateRepository
    {
        Task<State> GetStateById(int id);

        Task<IEnumerable<State>> GetAllStates();
    }
}