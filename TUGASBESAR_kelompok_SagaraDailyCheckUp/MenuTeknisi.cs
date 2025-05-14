using System.Text.Json;
using System.Text;

using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
namespace TUGASBESAR_kelompok_SagaraDailyCheckUp
{
    public class MenuTeknisi
    {

        private static readonly HttpClient client = new HttpClient();

        public static async Task<string?> LoginTeknisi()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN TEKNISI ===");
            Console.Write("Masukkan Username: ");
            string? username = Console.ReadLine();
            Console.Write("Masukkan Key: ");
            string? key = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(key))
            {
                Console.WriteLine("Username dan key harus diisi!");
                await Task.Delay(1000);
                return null;
            }

            try
            {
                var loginData = new { Username = username, Key = key };
                var jsonContent = JsonSerializer.Serialize(loginData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7119/api/Login/loginTeknisi", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseBody);

                    if (doc.RootElement.TryGetProperty("success", out var successElement) &&
                        successElement.GetBoolean())
                    {
                        if (doc.RootElement.TryGetProperty("key", out var keyElement))
                        {
                            string apiKey = keyElement.GetString();
                            Console.WriteLine("Login berhasil sebagai Teknisi.");
                            await Task.Delay(1000);
                            return apiKey;
                        }
                    }

                    if (doc.RootElement.TryGetProperty("message", out var messageElement))
                    {
                        Console.WriteLine($"Error: {messageElement.GetString()}");
                    }
                }
                else
                {
                    Console.WriteLine($"Login gagal. Status: {response.StatusCode}");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
            }

            await Task.Delay(1000);
            return null;
        }


        public async Task ShowMenuTeknisi()
        {
            string? apiKey = await LoginTeknisi();
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Akses ditolak. Hanya teknisi yang dapat mengakses menu ini.");
                Console.WriteLine("Tekan tombol apapun untuk kembali...");
                Console.ReadKey();
                PilihMenu.PilihMenu1();
                return;
            }

            // Set authorization header untuk request berikutnya
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var menuActions = new Dictionary<string, Func<Task>>
            {
                { "1", async () => await MenuDriver.TampilkanDataKerusakanDriver() },
                { "2", async () => { PilihMenu.PilihMenu1(); await Task.CompletedTask; } }
            };

            do
            {
                Console.Clear();
                Console.WriteLine("PILIH MENU:");
                Console.WriteLine("1. Lihat Data Kerusakan");
                Console.WriteLine("2. Keluar");

                var input = Console.ReadKey().Key.ToString().Substring(1);
                if (menuActions.ContainsKey(input))
                {
                    await menuActions[input]();
                    if (input == "2") break;
                }
                else
                {
                    Console.WriteLine("Pilihan tidak valid!");
                }

                Console.WriteLine("\nTekan tombol apapun untuk kembali ke menu...");
                Console.ReadKey();
            } while (true);
        }
    }
}
