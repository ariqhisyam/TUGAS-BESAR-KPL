using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Key { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class LoginDriver
    {
        private readonly HttpClient _httpClient;

        public LoginDriver(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Username atau password tidak boleh kosong."
                };
            }

            var payload = new { username, password };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/api/login", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var data = JsonSerializer.Deserialize<JsonElement>(responseString);
                    var key = data.GetProperty("key").GetString();

                    return new LoginResult
                    {
                        Success = true,
                        Key = key
                    };
                }

                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Login gagal. HTTP {response.StatusCode}: {responseString}"
                };
            }
            catch (HttpRequestException ex)
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = $"Terjadi kesalahan jaringan: {ex.Message}"
                };
            }
        }
    }
}
