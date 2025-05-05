using System;
using System.Threading.Tasks;

public static class Menu
{
    // Menu utama yang akan ditampilkan di CLI
    public static async Task ShowMenu()
    {
        string userInput;
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
            userInput = Console.ReadLine();

            // Memanggil metode berdasarkan pilihan pengguna
            switch (userInput)
            {
                case "1":
                    await CliProgram.CreateKey();
                    break;
                case "2":
                    await CliProgram.AddKendaraan();
                    break;
                case "3":
                    await CliProgram.UpdateKendaraan();
                    break;
                case "4":
                    await CliProgram.DeleteKendaraan();
                    break;
                case "5":
                    Console.WriteLine("Keluar dari aplikasi...");
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }

            Console.WriteLine("Tekan tombol apapun untuk melanjutkan...");
            Console.ReadKey();
        } while (userInput != "5");
    }
}
