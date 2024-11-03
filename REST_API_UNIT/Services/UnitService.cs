using Microsoft.Azure.Cosmos;
using REST_API_UNIT.Models;
namespace REST_API_UNIT.Services
{
    public class UnitService : IUnitService
    {
        private readonly Container _container;
        public UnitService(
            string conn,
            string key,
            string databaseName,
            string containerName)
        {
            var cosmosClient = new CosmosClient(conn, key, new CosmosClientOptions() { });
            _container = cosmosClient.GetContainer(databaseName, containerName);

        }

        public async Task<Unit> AddUnitAsync(Unit unit)
        {
            unit.Id = Guid.NewGuid().ToString();
            unit.LastUpdated = DateTime.Now;

            var res = await _container.CreateItemAsync(unit, new PartitionKey(unit.Id));
            return res.Resource;
        }

        public async Task<List<Unit>> GetAllUnitsAsync()
        {
            var query = _container
                .GetItemQueryIterator<Unit>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Unit>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Unit> GetUnitByIdAsync(string id)
        {
            try
            {
                var res = await _container.ReadItemAsync<Unit>(id.ToString(), new PartitionKey(id));
                return res;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Unit> UpdateUnitAsync(string id, Unit unit)
        {
            unit.LastUpdated = DateTime.Now;
            var res = await _container.UpsertItemAsync(unit, new PartitionKey(unit.Id));
            return res;
        }

        public async Task<bool> DeleteUnitAsync(string id)
        {
            var res = await _container.DeleteItemAsync<Unit>(id.ToString(), new PartitionKey(id));
            return res.Resource != null;
        }
    }
}
