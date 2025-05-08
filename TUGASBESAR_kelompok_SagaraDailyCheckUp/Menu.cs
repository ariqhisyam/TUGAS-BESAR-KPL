using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Text.RegularExpressions;
=======
using TUGASBESAR_kelompok_SagaraDailyCheckUp;
>>>>>>> AriqHisyamNabil

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

    // AUTOMATA ADE FATHIA NURAINI
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

        Console.WriteLine("Masukkan Plat Nomor (contoh: B 1221 SJT): "); // Bagian ini 
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
    // sampai sini mengalami perubahan (ADE FATHIA NURAINI)

    // AUTOMATA ADE FATHIA NURAINI
    private static async Task UpdateKendaraan()
    {
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

        var updatedKendaraan = new
        {
            Merek = merek,
            PlatNomor = platNomorBaru
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


    // AUTOMATA ADE FATHIA NURAINI
    private static async Task DeleteKendaraan()
    {
        Console.Write("Masukkan Plat Nomor Kendaraan yang ingin dihapus (format: B 1234 XYZ): ");
        string inputPlat = Console.ReadLine().ToUpper();

        // Otomatis format jika input rapat seperti B1234XYZ
        var regexFormat = new Regex(@"^([A-Z]{1,2})(\d{1,4})([A-Z]{1,3})$");
        if (regexFormat.IsMatch(inputPlat))
        {
            var match = regexFormat.Match(inputPlat);
            inputPlat = $"{match.Groups[1].Value} {match.Groups[2].Value} {match.Groups[3].Value}";
        }

        // Validasi akhir: harus sudah dalam format B 1234 XYZ
        string patternValid = @"^[A-Z]{1,2} [0-9]{1,4} [A-Z]{1,3}$";
        if (!Regex.IsMatch(inputPlat, patternValid))
        {
            Console.WriteLine("Format plat nomor tidak valid! Contoh yang benar: B 1234 XYZ");
            return;
        }

        try
        {
            var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKendaraan/{inputPlat}");

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
