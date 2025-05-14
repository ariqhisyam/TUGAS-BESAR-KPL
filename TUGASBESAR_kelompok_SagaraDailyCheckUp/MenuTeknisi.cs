namespace TUGASBESAR_kelompok_SagaraDailyCheckUp
{
    public class MenuTeknisi
    {
        public async Task ShowMenuTeknisi()
        {
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
