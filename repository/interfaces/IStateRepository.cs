using luchito_net.Models;
using luchito_net.Models.Entity;

namespace luchito_net.Repository.Interfaces
{
    public interface IStateRepository
    {
        Task<State> GetStateById(int id);


    }
}