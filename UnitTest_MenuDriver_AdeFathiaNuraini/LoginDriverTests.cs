using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;

namespace UnitTest_MenuDriver_AdeFathiaNuraini
{
    [TestClass]
    public class LoginDriverTests
    {
        [TestMethod]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"success\": true, \"key\": \"token123\"}")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var driver = new LoginDriver(httpClient);
            var result = await driver.LoginAsync("admin", "password");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("token123", result.Key);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public async Task LoginAsync_EmptyPassword_ReturnsError()
        {
            var driver = new LoginDriver(new HttpClient());
            var result = await driver.LoginAsync("user", "");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username atau password tidak boleh kosong.", result.ErrorMessage);
        }

        [TestMethod]
        public async Task LoginAsync_ServerReturnsUnauthorized_ReturnsFailure()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Unauthorized")
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://fakeapi.com")
            };

            var driver = new LoginDriver(httpClient);

            // Act
            var result = await driver.LoginAsync("user", "wrongpassword");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.ErrorMessage);
            Assert.IsTrue(result.ErrorMessage?.Contains("Unauthorized") == true); // ← Tambahkan baris ini
        }

    }
}
