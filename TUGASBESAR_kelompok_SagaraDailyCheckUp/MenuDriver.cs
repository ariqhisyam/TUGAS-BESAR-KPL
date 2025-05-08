using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    public static async Task ShowMenu()
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

}
