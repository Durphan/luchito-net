using luchito_net.Models.Entity;

namespace luchito_net.Service.Interfaces
{
    public interface IStateService
    {
        Task<State> GetStateById(int id);


    }
}