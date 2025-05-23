using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using Microsoft.AspNetCore.Mvc;
namespace UnitTestProject_Relingga
{
    [TestClass]
    public class AdminControllerTests
    {
        private AdminController controller;

        [TestInitialize]
        public void SetUp()
        {
            controller = new AdminController();
        }

        [TestMethod]
        public void Test_GetAllKendaraan_ReturnsOkResult()
        {
            var result = controller.GetKendaraan() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_GetKendaraanByPlatNomor_Existing_ReturnsOk()
        {
            string platNomor = "AB 123 CD";

            var result = controller.GetKendaraan(platNomor) as OkObjectResult;

            Assert.IsNotNull(result);
            var kendaraan = result.Value as Kendaraan;

            Assert.IsNotNull(kendaraan);
            Assert.AreEqual(platNomor, kendaraan.PlatNomor);
        }

        [TestMethod]
        public void Test_GetKendaraanByPlatNomor_NotExisting_ReturnsNotFound()
        {
            string platNomor = "ZZ 999 ZZ";

            var result = controller.GetKendaraan(platNomor) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Test_AddKendaraan_ReturnsCreatedAtAction()
        {
            var newKendaraan = new Kendaraan { Merek = "BRIO", PlatNomor = "TS 2025 EL" };

            var result = controller.AddKendaraan(newKendaraan) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);

            Assert.IsNotNull(kendaraan);
            Assert.AreEqual(newKendaraan.PlatNomor, kendaraan!.PlatNomor); 

        }

        [TestMethod]
        public void Test_UpdateKendaraan_Existing_ReturnsOk()
        {
            string platNomor = "AB 123 CD";
            var updatedKendaraan = new Kendaraan { Merek = "Toyota Updated", PlatNomor = "AB 123 CD" };

            var result = controller.UpdateKendaraan(platNomor, updatedKendaraan) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_UpdateKendaraan_NotExisting_ReturnsNotFound()
        {
            string platNomor = "ZZ 999 ZZ";
            var updatedKendaraan = new Kendaraan { Merek = "INNOVA", PlatNomor = "ZZ 999 ZZ" };

            var result = controller.UpdateKendaraan(platNomor, updatedKendaraan) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Test_DeleteKendaraan_Existing_ReturnsNoContent()
        {
            string platNomor = "EF 456 GH";

            var result = controller.DeleteKendaraan(platNomor) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void Test_DeleteKendaraan_NotExisting_ReturnsNotFound()
        {
            string platNomor = "ZZ 999 ZZ";

            var result = controller.DeleteKendaraan(platNomor) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
   
