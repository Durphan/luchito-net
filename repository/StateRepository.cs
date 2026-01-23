using Microsoft.EntityFrameworkCore;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Config.DataProvider;
using luchito_net.Models.Entity;

namespace luchito_net.Repository
{
    public class StateRepository(InitializeDatabase _context, ILogger<StateRepository> logger) : IStateRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<StateRepository> _logger = logger;

        public async Task<State> GetStateById(int id)
        {
            return await _context.State.FindAsync(id) ?? throw new Exception("State with ID " + id + " not found.");
        }
    }
}