using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Menu
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiBaseUrl = "https://localhost:7119/api/Admin";

    // table driven
    private static readonly Dictionary<string, Func<Task>> menuActions = new Dictionary<string, Func<Task>>()
    {
        { "1", CreateKey },
        { "2", AddKendaraan },
        { "3", UpdateKendaraan },
        { "4", DeleteKendaraan }
    };

    public static async Task ShowMenu()
    {
        string inputUser;
        do
        {
            Console.Clear();
            Console.WriteLine("PILIH MENU:");
            Console.WriteLine("1. Buat Key");
            Console.WriteLine("2. Tambah Data Kendaraan");
            Console.WriteLine("3. Update Kendaraan");
            Console.WriteLine("4. Hapus Kendaraan");
            Console.WriteLine("5. Keluar");
            Console.Write("Pilih menu (1-5): ");
            inputUser = Console.ReadLine();

            if (inputUser == "5")
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
        } while (inputUser != "5");
    }

    private static async Task CreateKey()
    {
        Console.WriteLine("Masukkan Username: ");
        string username = Console.ReadLine();

        Console.WriteLine("Masukkan Role: ");
        string role = Console.ReadLine();

        Console.WriteLine("Masukkan Key Value: ");
        string keyValue = Console.ReadLine();

        var key = new
        {
            Username = username,
            Role = role,
            KeyValue = keyValue
        };

        var jsonContent = JsonSerializer.Serialize(key);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{apiBaseUrl}/addKey", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Key berhasil dibuat.");
            else
                Console.WriteLine($"Gagal membuat key. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }

    private static string FormatPlatNomor(string platNomor)
    {
        // Menggunakan regex untuk memisahkan plat nomor berdasarkan pola yang umum
        // Contoh pola: "B 1221 SJT"
        var regex = new System.Text.RegularExpressions.Regex(@"([A-Z]+)(\d+)([A-Z]+)");
        var match = regex.Match(platNomor);

        if (match.Success)
        {
            return $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
        }

        return platNomor; // Jika format tidak sesuai, mengembalikan plat nomor seperti aslinya
    }

    private static async Task AddKendaraan()
    {
        Console.WriteLine("Masukkan Merek Kendaraan: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor (contoh: B1221SJT): ");
        string platNomor = Console.ReadLine();

        // Validasi bahwa plat nomor harus memiliki format dengan spasi, contoh: B 1234 XYZ
        string validPattern = @"^[A-Z]{1,2} [0-9]{1,4} [A-Z]{1,3}$";
        if (!Regex.IsMatch(platNomor.ToUpper(), validPattern))
        {
            Console.WriteLine("Format plat nomor tidak valid! Contoh yang benar: B 1234 XYZ");
            return;
        }


        // Memformat plat nomor
        platNomor = FormatPlatNomor(platNomor);

        var kendaraan = new
        {
            Merek = merek,
            PlatNomor = platNomor
        };

        var jsonContent = JsonSerializer.Serialize(kendaraan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync($"{apiBaseUrl}/addKendaraan", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Kendaraan berhasil ditambahkan.");
            else
                Console.WriteLine($"Gagal menambahkan kendaraan. Status: {response.StatusCode}");
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

        // Validasi plat nomor baru
        string pattern = @"^[A-Z]{1,2}[0-9]{1,4}[A-Z]{1,3}$";
        if (!Regex.IsMatch(platNomorBaru.ToUpper(), pattern))
        {
            Console.WriteLine("Format plat nomor tidak valid! Contoh yang benar: AB123CD");
            return;
        }

        var updatedKendaraan = new
        {
            Merek = merek,
            PlatNomor = platNomorBaru.ToUpper()
        };

        var jsonContent = JsonSerializer.Serialize(updatedKendaraan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PutAsync($"{apiBaseUrl}/updateKendaraan/{platNomor}", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Kendaraan berhasil diupdate.");
            else
                Console.WriteLine($"Gagal mengupdate kendaraan. Status: {response.StatusCode}");
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

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Kendaraan berhasil dihapus.");
            else
                Console.WriteLine($"Gagal menghapus kendaraan. Status: {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Terjadi kesalahan jaringan: " + ex.Message);
        }
    }
}
