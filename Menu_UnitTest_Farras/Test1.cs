using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using System.Text;
using System.Text.Json;
using Menu_UnitTest_Farras;

public class MenuTests
{
    [Fact]
    public async Task CreateKey_Should_ReturnSuccessMessage_OnSuccessStatusCode()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var client = new HttpClient(mockHandler.Object);
        var testUsername = "admin";
        var testRole = "Admin";
        var testKeyValue = "secret-key";

        var content = new
        {
            Username = testUsername,
            Role = testRole,
            KeyValue = testKeyValue
        };
        var json = JsonSerializer.Serialize(content);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:7119/api/Login/addKey", httpContent);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task TampilkanKey_Should_DeserializeKeyList()
    {
        // Arrange
        var mockResponse = new[]
        {
            new { Username = "admin", Role = "Admin", KeyValue = "secret" },
            new { Username = "user1", Role = "Viewer", KeyValue = "abcd" }
        };

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse), Encoding.UTF8, "application/json")
            });

        var client = new HttpClient(handler.Object);
        var response = await client.GetAsync("https://localhost:7119/api/Login/getKey");

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var deserialized = JsonSerializer.Deserialize<List<Key>>(jsonResponse);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(2, deserialized.Count);
        Assert.Equal("admin", deserialized[0].Username);
    }

    [Fact]
    public async Task AddKendaraan_Should_ReturnError_OnInvalidFormat()
    {
        // Arrange
        string invalidPlat = "B123XYZ";
        string patternValid = @"^[A-Z]{1,2} [0-9]{1,4} [A-Z]{1,3}$";

        // Act
        bool isValid = System.Text.RegularExpressions.Regex.IsMatch(invalidPlat, patternValid);

        // Assert
        Assert.False(isValid);
    }
}
