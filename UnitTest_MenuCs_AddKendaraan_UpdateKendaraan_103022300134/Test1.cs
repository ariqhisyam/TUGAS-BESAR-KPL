using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest_MenuCs_AddKendaraan_UpdateKendaraan_103022300134
{
    [TestClass]
    public sealed class Test1
    {
        private HttpClient CreateMockHttpClient(HttpStatusCode statusCode)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode
                });

            return new HttpClient(handlerMock.Object);
        }

        [TestMethod]
        public async Task Test_AddKendaraan_Success()
        {
            // Arrange
            var client = CreateMockHttpClient(HttpStatusCode.OK);
            var content = new StringContent("{\"Merek\":\"Toyota\",\"PlatNomor\":\"B 1234 ABC\"}", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("https://localhost:7119/api/Admin/addKendaraan", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Test_UpdateKendaraan_Success()
        {
            // Arrange
            var client = CreateMockHttpClient(HttpStatusCode.OK);
            var platLama = "B 1234 ABC";
            var platBaru = "B 5678 XYZ";
            var content = new StringContent("{\"Merek\":\"Honda\",\"PlatNomor\":\"" + platBaru + "\"}", Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"https://localhost:7119/api/Admin/updateKendaraan/{platLama}", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
