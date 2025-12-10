using Microsoft.EntityFrameworkCore;
using luchito_net.Models;
using luchito_net.Repository.Interfaces;
using luchito_net.Config.DataProvider;

namespace luchito_net.Repository
{
    public class StateRepository(InitializeDatabase _context, ILogger<StateRepository> logger) : IStateRepository
    {
        private readonly InitializeDatabase _context = _context;
        private readonly ILogger<StateRepository> _logger = logger;

        public async Task<State> GetStateById(int id)
        {
            try
            {
                return await _context.Set<State>().FindAsync(id) ?? throw new Exception("State with ID " + id + " not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetStateById for state ID {StateId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<State>> GetAllStates()
        {
            try
            {
                return await _context.Set<State>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllStates");
                throw;
            }
        }
    }
}