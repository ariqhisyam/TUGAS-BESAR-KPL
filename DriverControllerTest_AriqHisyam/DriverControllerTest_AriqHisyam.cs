using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DriverControllerTest_AriqHisyam
{
    [TestClass]
    public class DriverControllerTests
    {
        private DriverController _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new DriverController();
        }
        [TestMethod]
        public void GetKerusakan_ReturnsListOfKerusakan()
        {
            // Act
            var result = _controller.GetKerusakan() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var list = result.Value as List<Kerusakan>;
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void AddKerusakan_ShouldAddKerusakan()
        {
            // Arrange
            var newKerusakan = new Kerusakan
            {
                Merek = "Suzuki",
                PlatNomor = "ZZ 999 ZZ",
                Kendala = "Rem blong"
            };

            // Act
            var result = _controller.AddKerusakan(newKerusakan) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("GetKerusakan", result.ActionName);
        }

        [TestMethod]
        public void UpdateKerusakan_ShouldUpdateKendala()
        {
            // Arrange
            var updatedKerusakan = new Kerusakan { Kendala = "Mesin mati total" };

            // Act
            var result = _controller.UpdateKerusakan("AB 123 CD", updatedKerusakan) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Kerusakan berhasil diperbarui!", result.Value);
        }

        [TestMethod]
        public void DeleteKerusakan_ShouldRemoveKerusakan()
        {
            // Act
            var result = _controller.DeleteKerusakan("EF 456 GH");

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
