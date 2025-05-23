using Microsoft.VisualStudio.TestTools.UnitTesting;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TestProject_Relinggaa
{
    [TestClass]
    public class TableDrivenTest
    {
        private AdminController controller;

        [TestInitialize]
        public void SetUp()
        {
            controller = new AdminController();
        }

        [TestMethod]
        public void Test_UpdateKendaraan_TableDriven()
        {
            var testCases = new[]
            {
                new { PlatNomor="AB 123 CD", NewMerek="Toyota Updated", ExpectedStatus=200, Description="Valid update" },
                new { PlatNomor="XY 789 ZT", NewMerek="BMW Updated", ExpectedStatus=200, Description="Valid update" },
                new { PlatNomor="NOT EXIST", NewMerek="Invalid", ExpectedStatus=404, Description="Invalid update - kendaraan tidak ada" },
            };

            foreach (var testCase in testCases)
            {
                var updatedKendaraan = new Kendaraan { Merek = testCase.NewMerek, PlatNomor = testCase.PlatNomor };
                var result = controller.UpdateKendaraan(testCase.PlatNomor, updatedKendaraan);

                if (testCase.ExpectedStatus == 200)
                {
                    Assert.IsInstanceOfType(result, typeof(OkObjectResult), testCase.Description);
                    var okResult = result as OkObjectResult;
                    Assert.AreEqual(200, okResult.StatusCode);
                }
                else if (testCase.ExpectedStatus == 404)
                {
                    Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult), testCase.Description);
                    var notFoundResult = result as NotFoundObjectResult;
                    Assert.AreEqual(404, notFoundResult.StatusCode);
                }
            }
        }
    }
}