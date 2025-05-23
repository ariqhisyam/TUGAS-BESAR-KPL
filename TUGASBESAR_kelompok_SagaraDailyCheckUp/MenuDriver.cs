using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;
using System.Text;

public static class MenuDriver
{
    private static readonly HttpClient client = new HttpClient();
    private const string apiBaseUrl = "https://localhost:7119/api/Driver";

    // table driven
    private static readonly Dictionary<string, Func<Task>> menuActions = new Dictionary<string, Func<Task>>()
    {
        { "1", InputDataKerusakan },
        { "2", EditDataKerusakan },
        { "3", HapusDataKerusakan },
        { "4", TampilkanDataKerusakanDriver },
        

    };


    public static async Task<string?> LoginDriver()
    {
        Console.Clear();
        Console.WriteLine("=== LOGIN DRIVER ===");
        Console.Write("Masukkan Username: ");
        string? username = Console.ReadLine();
        Console.Write("Masukkan Key: ");
        string? key = Console.ReadLine();

        // Validate input
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

            // Correct endpoint URL
            var response = await client.PostAsync("https://localhost:7119/api/Login/loginDriver", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseBody);

                // Check if login was successful
                if (doc.RootElement.TryGetProperty("success", out var successElement) &&
                    successElement.GetBoolean())
                {
                    // Get the API key
                    if (doc.RootElement.TryGetProperty("key", out var keyElement))
                    {
                        string apiKey = keyElement.GetString();
                        Console.WriteLine("Login berhasil sebagai Driver.");
                        await Task.Delay(1000);
                        return apiKey;
                    }
                }

                // If success=false or key not found
                if (doc.RootElement.TryGetProperty("message", out var messageElement))
                {
                    Console.WriteLine($"Login gagal: {messageElement.GetString()}");
                }
                else
                {
                    Console.WriteLine("Login gagal. Format respons tidak valid.");
                }
            }
            else
            {
                // Handle non-success status codes
                string errorDetail = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Login gagal. Status: " + response.StatusCode);

                try
                {
                    // Try to parse error message
                    var errorDoc = JsonDocument.Parse(errorDetail);
                    if (errorDoc.RootElement.TryGetProperty("message", out var errorMessage))
                    {
                        Console.WriteLine($"Detail error: {errorMessage.GetString()}");
                    }
                    else
                    {
                        Console.WriteLine($"Detail error: {errorDetail}");
                    }
                }
                catch
                {
                    Console.WriteLine($"Detail error: {errorDetail}");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Gagal terhubung ke server: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
        }

        await Task.Delay(1000);
        return null;
    }


    //dsini
    public static async Task ShowDriver()
    {
        // Tambahkan login sebelum menu
        string? apiKey = await LoginDriver();
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Akses ditolak. Hanya driver yang dapat mengakses menu ini.");
            Console.WriteLine("Tekan tombol apapun untuk kembali...");
            Console.ReadKey();
            PilihMenu.PilihMenu1();
            return;
        }

        string inputUser;
        do
        {
            Console.Clear();
            Console.WriteLine("PILIH MENU:");
            Console.WriteLine("1. Input Data Kerusakan");
            Console.WriteLine("2. Edit Data Kerusakan");
            Console.WriteLine("3. Hapus Data Kerusakan");
            Console.WriteLine("4. Tampilkan Data Kerusakan Driver");
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


    public static async Task InputDataKerusakan()
    {
        // Menampilkan daftar kendaraan yang ada dari AdminController
        Console.WriteLine("Menampilkan daftar kendaraan yang tersedia...");
        var kendaraanResponse = await client.GetAsync("https://localhost:7119/api/Admin/getKendaraan");  // Pastikan menggunakan API Admin untuk kendaraan

        if (kendaraanResponse.IsSuccessStatusCode)
        {
            var jsonKendaraanResponse = await kendaraanResponse.Content.ReadAsStringAsync();
            var kendaraanList = JsonSerializer.Deserialize<List<Kendaraan>>(jsonKendaraanResponse);

            if (kendaraanList != null && kendaraanList.Count > 0)
            {
                Console.WriteLine("Daftar Kendaraan Tersedia:");
                foreach (var kendaraan in kendaraanList)
                {
                    Console.WriteLine($"Plat Nomor: {kendaraan.PlatNomor}, Merek: {kendaraan.Merek}");
                }

                // Meminta pengguna untuk memilih plat nomor
                Console.WriteLine("Masukkan Plat Nomor Kendaraan untuk input kerusakan: ");
                string platNomor = Console.ReadLine();

                // Verifikasi apakah plat nomor yang dimasukkan ada dalam daftar kendaraan
                var selectedKendaraan = kendaraanList.FirstOrDefault(k => k.PlatNomor == platNomor);
                if (selectedKendaraan != null)
                {
                    Console.WriteLine($"Kendaraan yang dipilih: {selectedKendaraan.PlatNomor} - {selectedKendaraan.Merek}");

                    // Memasukkan data kerusakan
                    Console.WriteLine("Masukkan Kendala: ");
                    string kendala = Console.ReadLine();

                    Console.WriteLine("Masukkan Catatan: ");
                    string catatan = Console.ReadLine();

                    var kerusakan = new
                    {
                        merek = selectedKendaraan.Merek,
                        platNomor = platNomor,
                        kendala = kendala,
                        catatan = catatan
                    };

                    var jsonContent = JsonSerializer.Serialize(kerusakan);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{apiBaseUrl}/addKerusakan", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Data Kerusakan Berhasil Dibuat, dan telah dikirim ke admin dan teknisi.");
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Gagal membuat data kerusakan. Status: {response.StatusCode}, Error: {errorResponse}");
                    }
                }
                else
                {
                    Console.WriteLine("Plat Nomor tidak ditemukan. Silakan coba lagi.");
                }
            }
            else
            {
                Console.WriteLine("Tidak ada kendaraan yang tersedia.");
            }
        }
        else
        {
            Console.WriteLine($"Gagal mengambil data kendaraan. Status: {kendaraanResponse.StatusCode}");
        }
    }


    //Membenari dan menambahkan automata Nur Ahmadi Aditya Nanda
    public static async Task EditDataKerusakan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin diedit: ");
        string platNomor = Console.ReadLine();

        // Mendapatkan data kerusakan berdasarkan plat nomor
        var response = await client.GetAsync($"{apiBaseUrl}/getKerusakan");
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var kerusakanList = JsonSerializer.Deserialize<List<Kerusakan>>(jsonResponse);

            // Cari kerusakan berdasarkan plat nomor
            var kerusakan = kerusakanList?.FirstOrDefault(k => k.PlatNomor == platNomor);
            if (kerusakan != null)
            {
                Console.WriteLine($"Data Kerusakan yang ditemukan: {kerusakan.PlatNomor}, Kendala: {kerusakan.Kendala}, Catatan: {kerusakan.Catatan}");

                // memperbarui kendala atau catatan
                Console.WriteLine("Masukkan Kendala Baru (kosongkan jika tidak ingin mengubah): ");
                string kendalaBaru = Console.ReadLine();
                if (!string.IsNullOrEmpty(kendalaBaru)) kerusakan.Kendala = kendalaBaru;

                Console.WriteLine("Masukkan Catatan Baru (kosongkan jika tidak ingin mengubah): ");
                string catatanBaru = Console.ReadLine();
                if (!string.IsNullOrEmpty(catatanBaru)) kerusakan.Catatan = catatanBaru;

                var jsonContent = JsonSerializer.Serialize(kerusakan);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var updateResponse = await client.PutAsync($"{apiBaseUrl}/updateKerusakan/{platNomor}", content);
                if (updateResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data Kerusakan berhasil diperbarui!");
                }
                else
                {
                    Console.WriteLine($"Gagal memperbarui data kerusakan. Status: {updateResponse.StatusCode}");
                }
            }
            else
            {
                Console.WriteLine("Plat Nomor tidak ditemukan.");
            }
        }
        else
        {
            Console.WriteLine($"Gagal mengambil data kerusakan. Status: {response.StatusCode}");
        }
    }


    public static async Task HapusDataKerusakan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin dihapus: ");
        string platNomor = Console.ReadLine();
        var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKerusakan/{platNomor}");
    }

    public static async Task TampilkanDataKerusakanDriver()
    {
        Console.Clear();
        try
        {
            var response = await client.GetAsync($"{apiBaseUrl}/getKerusakan");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var kerusakanList = JsonSerializer.Deserialize<List<Kerusakan>>(jsonResponse);
                if (kerusakanList != null && kerusakanList.Count > 0)
                {
                 
                    Console.WriteLine("Data Kerusakan Driver:");
                   
                    foreach (var kerusakan in kerusakanList)
                    {

                     
                        Console.WriteLine("Data Kerusakan :");
                        Console.WriteLine($"Plat Nomor: {kerusakan.PlatNomor}, Kendala: {kerusakan.Kendala}, Catatan: {kerusakan.Catatan}");
                    }
                }
                else
                {
                    Console.WriteLine("Tidak ada data kerusakan.");
                }
            }
            else
            {
                Console.WriteLine($"Gagal mengambil data kerusakan driver. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Terjadi kesalahan: " + ex.Message);
        }
    }
}
   

  

