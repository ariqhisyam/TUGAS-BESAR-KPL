using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TUGASBESAR_kelompok_SagaraDailyCheckUp;

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

        var response = await client.PostAsync($"{apiBaseUrl}/addKey", content);

  
    }

    // Method untuk menambahkan kendaraan
    private static async Task AddKendaraan()
    {
        Console.WriteLine("Masukkan Merek Kendaraan: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor: ");
        string platNomor = Console.ReadLine();

        var kendaraan = new
        {
            Merek = merek,
            PlatNomor = platNomor
        };

        var jsonContent = JsonSerializer.Serialize(kendaraan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{apiBaseUrl}/addKendaraan", content);

     
    }

    // Method untuk mengupdate kendaraan
    private static async Task UpdateKendaraan()
    {
        Console.WriteLine("Masukkan Plat Nomor Kendaraan yang ingin diupdate: ");
        string platNomor = Console.ReadLine();

        Console.WriteLine("Masukkan Merek Kendaraan Baru: ");
        string merek = Console.ReadLine();

        Console.WriteLine("Masukkan Plat Nomor Baru: ");
        string platNomorBaru = Console.ReadLine();

        var updatedKendaraan = new
        {
            Merek = merek,
            PlatNomor = platNomorBaru
        };

        var jsonContent = JsonSerializer.Serialize(updatedKendaraan);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"{apiBaseUrl}/updateKendaraan/{platNomor}", content);

    }

    // Method untuk menghapus kendaraan
    private static async Task DeleteKendaraan()
    {
        Console.Write("Masukkan Plat Nomor Kendaraan yang ingin dihapus: ");
        string platNomor = Console.ReadLine();

        var response = await client.DeleteAsync($"{apiBaseUrl}/deleteKendaraan/{platNomor}");

    }
}
