using Microsoft.Azure.Cosmos;
using REST_API_UNIT.Models;

namespace REST_API_UNIT.Services
{
    public interface IUnitService
    {
        Task<Unit> AddUnitAsync(Unit unit);
        Task<List<Unit>> GetAllUnitsAsync();
        Task<Unit> GetUnitByIdAsync(string id);
        Task<Unit> UpdateUnitAsync(string id, Unit unit);
        Task<bool> DeleteUnitAsync(string id);
    }
}
