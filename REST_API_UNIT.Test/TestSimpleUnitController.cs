using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REST_API_UNIT.Models;

namespace REST_API_UNIT.Test
{
    [TestClass]
    public class TestSimpleProductController
    {
        [TestMethod]
        public void GetAllUnits_ShouldReturnAllUnits()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var result = controller.GetAllUnits() as List<Unit>;
            Assert.AreEqual(testUnits.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllUnitsAsync_ShouldReturnAllUnits()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var result = await controller.GetAllUnitsAsync() as List<Unit>;
            Assert.AreEqual(testUnits.Count, result.Count);
        }

        [TestMethod]
        public void GetUnit_ShouldReturnCorrectUnit()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var result = controller.GetUnit(testUnits[3].Id) as OkNegotiatedContentResult<Unit>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testUnits[3].Name, result.Content.Name);
        }

        [TestMethod]
        public async Task GetUnitAsync_ShouldReturnCorrectUnit()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var result = await controller.GetUnitAsync(testUnits[3].Id) as OkNegotiatedContentResult<Unit>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testUnits[3].Name, result.Content.Name);
        }

        [TestMethod]
        public void GetUnit_ShouldNotFindUnit()
        {
            var controller = new SimpleUnitController(GetTestUnits());

            var result = controller.GetUnit("non-existing-id");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void PostUnit_ShouldReturnCreated()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var newUnit = new Unit
            {
                Id = Guid.NewGuid().ToString(),
                Name = "NewUnit",
                IsActive = true,
                Type = "Type3",
                LastUpdated = DateTime.Now
            };

            var result = controller.PostUnit(newUnit) as CreatedAtRouteNegotiatedContentResult<Unit>;

            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultApi", result.RouteName);
            Assert.AreEqual(newUnit.Id, result.RouteValues["id"]);
            Assert.AreEqual(newUnit.Name, result.Content.Name);
        }

        [TestMethod]
        public void PostUnit_ShouldReturnConflict_WhenIdExists()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var existingUnit = testUnits[0]; // Trying to post a unit with an existing ID

            var result = controller.PostUnit(existingUnit);

            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [TestMethod]
        public void PutUnit_ShouldReturnOk()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var updatedUnit = new Unit
            {
                Id = testUnits[0].Id,
                Name = "UpdatedUnit",
                IsActive = false,
                Type = "TypeUpdated",
                LastUpdated = DateTime.Now
            };

            var result = controller.PutUnit(testUnits[0].Id, updatedUnit) as OkNegotiatedContentResult<Unit>;

            Assert.IsNotNull(result);
            Assert.AreEqual(updatedUnit.Name, result.Content.Name);
            Assert.AreEqual(updatedUnit.IsActive, result.Content.IsActive);
            Assert.AreEqual(updatedUnit.Type, result.Content.Type);
        }

        [TestMethod]
        public void PutUnit_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            var testUnits = GetTestUnits();
            var controller = new SimpleUnitController(testUnits);

            var updatedUnit = new Unit
            {
                Id = "non-existing-id",
                Name = "UpdatedUnit",
                IsActive = false,
                Type = "TypeUpdated",
                LastUpdated = DateTime.Now
            };

            var result = controller.PutUnit("non-existing-id", updatedUnit);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        private List<Unit> GetTestUnits()
        {
            var testUnits = new List<Unit>
            {
                new Unit { Id = Guid.NewGuid().ToString(), Name = "Unit1", IsActive = true, Type = "Type1", LastUpdated = DateTime.Now },
                new Unit { Id = Guid.NewGuid().ToString(), Name = "Unit2", IsActive = true, Type = "Type2", LastUpdated = DateTime.Now },
                new Unit { Id = Guid.NewGuid().ToString(), Name = "Unit3", IsActive = true, Type = "Type2", LastUpdated = DateTime.Now },
                new Unit { Id = Guid.NewGuid().ToString(), Name = "Unit4", IsActive = true, Type = "Type2", LastUpdated = DateTime.Now }
            };

            return testUnits;
        }
    }
}
