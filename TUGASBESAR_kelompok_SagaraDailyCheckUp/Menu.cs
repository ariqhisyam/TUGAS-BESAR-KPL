using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

public static class Menu
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiBaseUrl = "https://localhost:7119/api/Admin";

    private static Dictionary<string, Func<Task>> menuActions = new();
    private static Dictionary<string, string> menuLabels = new();
    private static readonly Dictionary<string, Func<Task>> methodMap = new()
    {
        { "CreateKey", CreateKey },
        { "AddKendaraan", AddKendaraan },
        { "UpdateKendaraan", UpdateKendaraan },
        { "DeleteKendaraan", DeleteKendaraan }
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
                Console.WriteLine("Keluar...");
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
        Console.WriteLine("Masukkan Merek Kendaraan: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor: ");
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

    private static async Task UpdateKendaraan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin diupdate: ");
        string platNomor = Console.ReadLine();

        Console.WriteLine("Masukkan Merek Kendaraan Baru: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor Baru: ");
        string platNomorBaru = Console.ReadLine();

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
        Console.Write("Masukkan Plat Nomor Kendaraan yang ingin dihapus: ");
        string platNomor = Console.ReadLine();

        try
        {
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
}
