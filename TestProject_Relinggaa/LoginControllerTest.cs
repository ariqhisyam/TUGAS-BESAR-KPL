using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Controllers;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

namespace TestProject_Relinggaa
{
    [TestClass]
    public class LoginControllerTest
    {
        private LoginController controller;

        [TestInitialize]
        public void Setup()
        {
            controller = new LoginController();
        }

        [TestMethod]
        public void Test_GetKey_ReturnsAllKeys()
        {
            var result = controller.GetKey() as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_AddKey_ReturnsCreatedAtAction()
        {
            var newKey = new Key { Username = "testuser", Role = "user", KeyValue = "test123" };

            var result = controller.AddKey(newKey) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);

            var createdKey = result.Value as Key;
            Assert.AreEqual("testuser", createdKey.Username);
        }

        [TestMethod]
        public void Test_UpdateKey_ExistingUser_ReturnsOk()
        {
            var updatedKey = new Key { Username = "admin", Role = "admin", KeyValue = "newkey" };

            var result = controller.UpdateKey("admin", updatedKey) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_UpdateKey_NonExistingUser_ReturnsNotFound()
        {
            var updatedKey = new Key { Username = "nonexistent", Role = "user", KeyValue = "key" };

            var result = controller.UpdateKey("nonexistent", updatedKey) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Test_DeleteKey_ExistingUser_ReturnsNoContent()
        {
            var result = controller.DeleteKey("rey") as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void Test_DeleteKey_NonExistingUser_ReturnsNotFound()
        {
            var result = controller.DeleteKey("unknown") as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public void Test_LoginAdmin_Success_ReturnsOk()
        {
            var request = new LoginRequest { Username = "admin", Key = "12345" };

            var result = controller.LoginAdmin(request) as OkObjectResult;

     
     
        }

        [TestMethod]
        public void Test_LoginDriver_Failure_ReturnsUnauthorized()
        {
            var request = new LoginRequest { Username = "rey", Key = "wrongkey" };

            var result = controller.loginDriver(request) as UnauthorizedObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void Test_LoginTeknisi_Success_ReturnsOk()
        {
            var request = new LoginRequest { Username = "teknisi", Key = "123456" };

            var result = controller.LoginTeknisi(request) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Test_GetTanggalHariIni_ReturnsFormattedDate()
        {
            var result = controller.GetTanggalHariIni() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("Hari ini"));
        }
    }
}
