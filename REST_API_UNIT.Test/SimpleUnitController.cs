using System.Web.Http;
using REST_API_UNIT.Models;

namespace REST_API_UNIT.Test
{
    public class SimpleUnitController : ApiController
    {
        private List<Unit> units = new List<Unit>();

        public SimpleUnitController() { }

        public SimpleUnitController(List<Unit> units)
        {
            this.units = units;
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            return units;
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await Task.FromResult(GetAllUnits());
        }

        public IHttpActionResult GetUnit(string id)
        {
            var unit = units.FirstOrDefault((u) => u.Id.Equals(id));
            if (unit == null)
            {
                return NotFound();
            }
            return Ok(unit);
        }

        public async Task<IHttpActionResult> GetUnitAsync(string id)
        {
            return await Task.FromResult(GetUnit(id));
        }

        public IHttpActionResult PostUnit(Unit unit)
        {
            if (unit == null)
            {
                return BadRequest("Unit cannot be null");
            }

            if (units.Any(u => u.Id == unit.Id))
            {
                return Conflict();
            }

            units.Add(unit);
            return CreatedAtRoute("DefaultApi", new { id = unit.Id }, unit);
        }

        public IHttpActionResult PutUnit(string id, Unit updatedUnit)
        {
            var unit = units.FirstOrDefault((u) => u.Id.Equals(id));

            if (unit == null)
            {
                return NotFound();
            }
            unit.Name = updatedUnit.Name;
            unit.IsActive = updatedUnit.IsActive;
            unit.Type = updatedUnit.Type;
            unit.LastUpdated = updatedUnit.LastUpdated;

            return Ok(unit);
        }

    }
}
