using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;
using System.Text.RegularExpressions;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

public static class Menu
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiBaseUrl = "https://localhost:7119/api/Admin";

    private static Dictionary<string, Func<Task>> menuActions = new();
    private static Dictionary<string, string> menuLabels = new();
    private static object platNomor;
    private static readonly Dictionary<string, Func<Task>> methodMap = new()
    {
        { "CreateKey", CreateKey },
        { "AddKendaraan", AddKendaraan },
        { "UpdateKendaraan", UpdateKendaraan },
        { "DeleteKendaraan", DeleteKendaraan },
            { "TampilkanDataKendaraan", TampilkanDataKendaraan },
         { "TampilkanDataKerusakaan", TampilkanDataKerusakan }
    };

    public static async Task ShowMenu()
    {
        await LoadMenuFromJson("menu.json");

        string inputUser;
        do
        {
            Console.Clear();
            Console.WriteLine("=== PILIH MENU ===");
            foreach (var item in menuLabels)
            {
                Console.WriteLine($"{item.Key}. {FormatMenuLabel(item.Value)}");
            }
            Console.WriteLine("0. Keluar");
            Console.Write("Pilih menu: ");
            inputUser = Console.ReadLine();

            if (inputUser == "0")
            {
                PilihMenu.PilihMenu1();
                break;
            }

            if (menuActions.ContainsKey(inputUser))
            {
                await menuActions[inputUser]();
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid!");
            }

            Console.WriteLine("Tekan tombol apapun untuk melanjutkan...");
            Console.ReadKey();
        } while (true);
    }

    public static async Task LoadMenuFromJson(string path)
    {
        try
        {
            var json = await File.ReadAllTextAsync(path);
            var rawMenu = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            foreach (var item in rawMenu)
            {
                if (methodMap.TryGetValue(item.Value, out var method))
                {
                    menuActions[item.Key] = method;
                    menuLabels[item.Key] = item.Value;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Gagal memuat menu dari JSON: " + ex.Message);
        }
    }

    private static string FormatMenuLabel(string key)
    {
        return key switch
        {
            "CreateKey" => "Buat Key",
            "AddKendaraan" => "Tambah Kendaraan",
            "UpdateKendaraan" => "Update Kendaraan",
            "DeleteKendaraan" => "Hapus Kendaraan",
            _ => key
        };
    }

    private static async Task CreateKey()
    {
        Console.Clear();
        Console.WriteLine("Masukkan Username: ");
        string username = Console.ReadLine();

        Console.WriteLine("Masukkan Role: ");
        string role = Console.ReadLine();

        Console.WriteLine("Masukkan Key Value: ");
        string keyValue = Console.ReadLine();

        var key = new { Username = username, Role = role, KeyValue = keyValue };
        var jsonContent = JsonSerializer.Serialize(key);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{apiBaseUrl}/addKey", content);
            Console.WriteLine(response.IsSuccessStatusCode
                ? "Key berhasil dibuat."
                : $"Gagal membuat key. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }

    private static async Task AddKendaraan()
    {
        Console.Clear();
        Console.WriteLine("Masukkan Merek Kendaraan: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor (contoh: B 1221 SJT): "); // Bagian ini 
        string platNomor = Console.ReadLine();

        var kendaraan = new { Merek = merek, PlatNomor = platNomor };
        var content = new StringContent(JsonSerializer.Serialize(kendaraan), System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{apiBaseUrl}/addKendaraan", content);
            Console.WriteLine(response.IsSuccessStatusCode
                ? "Kendaraan berhasil ditambahkan."
                : $"Gagal menambahkan kendaraan. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }
    // sampai sini mengalami perubahan (ADE FATHIA NURAINI)

    private static async Task UpdateKendaraan()
    {
        Console.Clear();
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin diupdate (format: B 1234 XYZ): ");
        string platNomor = Console.ReadLine().ToUpper();

        // Format otomatis jika input rapat (B1234XYZ)
        var regexFormat = new Regex(@"^([A-Z]{1,2})(\d{1,4})([A-Z]{1,3})$");
        if (regexFormat.IsMatch(platNomor))
        {
            var match = regexFormat.Match(platNomor);
            platNomor = $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
        }

        // Validasi akhir
        string patternValid = @"^[A-Z]{1,2} [0-9]{1,4} [A-Z]{1,3}$";
        if (!Regex.IsMatch(platNomor, patternValid))
        {
            Console.WriteLine("Format plat nomor tidak valid! Contoh yang benar: B 1234 XYZ");
            return;
        }

        Console.WriteLine("Masukkan Merek Kendaraan Baru: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor Baru (format: B 1234 XYZ): ");
        string platNomorBaru = Console.ReadLine().ToUpper();

        // Format otomatis untuk input baru
        if (regexFormat.IsMatch(platNomorBaru))
        {
            var match = regexFormat.Match(platNomorBaru);
            platNomorBaru = $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
        }

        if (!Regex.IsMatch(platNomorBaru, patternValid))
        {
            Console.WriteLine("Format plat nomor baru tidak valid! Contoh: B 1234 XYZ");
            return;
        }

        var updatedKendaraan = new { Merek = merek, PlatNomor = platNomorBaru };
        var content = new StringContent(JsonSerializer.Serialize(updatedKendaraan), System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PutAsync($"{apiBaseUrl}/updateKendaraan/{platNomor}", content);
            Console.WriteLine(response.IsSuccessStatusCode
                ? "Kendaraan berhasil diupdate."
                : $"Gagal mengupdate kendaraan. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }

    private static async Task DeleteKendaraan()
    {
        Console.Write("Masukkan Plat Nomor Kendaraan yang ingin dihapus (format: B 1234 XYZ): ");
        string inputPlat = Console.ReadLine().ToUpper();

        try
        {
            Console.Clear();
            var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKendaraan/{platNomor}");
            Console.WriteLine(response.IsSuccessStatusCode
                ? "Kendaraan berhasil dihapus."
                : $"Gagal menghapus kendaraan. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }

    private static async Task TampilkanDataKendaraan()
    {
        try
        {
            var response = await client.GetAsync($"{apiBaseUrl}/getKendaraan");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
           

                var kendaraanList = JsonSerializer.Deserialize<List<Kendaraan>>(jsonResponse);

                // Debugging: Cek apakah list terisi
                if (kendaraanList != null && kendaraanList.Count > 0)

                {
                    Console.Clear();
                    Console.WriteLine("Data Kendaraan:");
                    foreach (var kendaraan in kendaraanList)
                    {
                        // Debugging untuk menampilkan nilai setiap kendaraan
                        Console.WriteLine($"Merek: {kendaraan.Merek}, Plat Nomor: {kendaraan.PlatNomor}");
                    }
                }
                else
                {
                    Console.WriteLine("Tidak ada data kendaraan.");
                }
            }
            else
            {
                Console.WriteLine($"Gagal mengambil data kendaraan. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Terjadi kesalahan: " + ex.Message);
        }
    }




    // PENAMBAHAN tampilkan data kendaraan, tampilkan data kerusakan di driver, tampilkan data kerusakan yang dari driver ke admin (ADE FATHIA NURAINI AND RELINGGA)
    private static async Task TampilkanDataKerusakan()
    {
        Console.Clear();
        await MenuDriver.TampilkanDataKerusakanDriver();
    }

    // PENAMBAHAN tampilkan data kendaraan, tampilkan data kerusakan di driver, tampilkan data kerusakan yang dari driver ke admin (ADE FATHIA NURAINI)
    public static async Task selectMenu()
    {
        string inputUser;
        do
        {
            Console.Clear();
            Console.WriteLine("=== PILIH MENU ===");
            Console.WriteLine("1. Tampilkan Data Kendaraan");
            Console.WriteLine("2. Tampilkan Data Kerusakan");
            Console.WriteLine("0. Keluar");
            Console.Write("Pilih menu: ");
            inputUser = Console.ReadLine();

            if (inputUser == "0")
            {
                PilihMenu.PilihMenu1();
                break;
            }

            if (inputUser == "1")
            {
                await TampilkanDataKendaraan();
            }
            else if (inputUser == "2")
            {
                await TampilkanDataKerusakan();
            }
            else
            {
                Console.WriteLine("Pilihan tidak valid!");
            }

            Console.WriteLine("Tekan tombol apapun untuk melanjutkan...");
            Console.ReadKey();
        } while (true);
    }


}
