using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;

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

    };
//dsini
    public static async Task ShowDriver()
    {
        string inputUser;
        do
        {
            Console.Clear();
            Console.WriteLine("PILIH MENU:");
            Console.WriteLine("1. Input Data Kerusakan");
            Console.WriteLine("2. Edit Data Kerusakan");
            Console.WriteLine("3. Hapus Data Kerusakan");
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

    public static async Task InputDataKerusakan() { 
        Console.WriteLine("Masukkan Merek Kendaraan: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor: ");
        string platNomor = Console.ReadLine();

        Console.WriteLine("Masukkan Kendala: ");
        string kendala = Console.ReadLine();

        Console.WriteLine("Masukkan Catatan: ");
        string catatan = Console.ReadLine();

        var kerusakan = new
        {
            Merek = merek,
            PlatNomor = platNomor,
            Kendala = kendala,
            Catatan = catatan
        };



        var jsonContent = JsonSerializer.Serialize(kerusakan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{apiBaseUrl}/addKerusakan", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Data Kerusakan Berhasil Di Buat,dan telah ke kirim ke admin");
        }
        else
        {
            Console.WriteLine($"Gagal membuat data kerusakan. Status: {response.StatusCode}");
        }
        

    }

    public static async Task EditDataKerusakan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin diedit: ");
        string platNomor = Console.ReadLine();
        Console.WriteLine("Masukkan Kendala Baru: ");
        string kendala = Console.ReadLine();
        var kerusakan = new
        {
            Kendala = kendala
        };
        var jsonContent = JsonSerializer.Serialize(kerusakan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"{apiBaseUrl}/updateKerusakan/{platNomor}", content);
    }

    public static async Task HapusDataKerusakan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin dihapus: ");
        string platNomor = Console.ReadLine();
        var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKerusakan/{platNomor}");
    }

    public static async Task TampilkanDataKerusakanDriver()
    {
        try
        {
            var response = await client.GetAsync($"{apiBaseUrl}/getKerusakanDriver");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var kerusakanList = JsonSerializer.Deserialize<List<Kerusakan>>(jsonResponse);

                if (kerusakanList != null && kerusakanList.Count > 0)
                {
                    Console.WriteLine("Data Kerusakan Driver:");
                    foreach (var kerusakan in kerusakanList)
                    {
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
