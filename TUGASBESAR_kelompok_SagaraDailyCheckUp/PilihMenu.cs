namespace TUGASBESAR_kelompok_SagaraDailyCheckUp
{
    public class PilihMenu
    {
        public static void PilihMenu1()
        {
            Console.Clear();
            Console.WriteLine("Pilih menu:");
            Console.WriteLine("1. Menu Admin");
            Console.WriteLine("2. Menu Driver");
            Console.WriteLine("3. Keluar");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Menu.ShowMenu().Wait();
                    break;
                case "2":
                    MenuDriver.ShowDriver().Wait();
                    break;
                case "3":
                    Console.WriteLine("Keluar...");
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }
        }
    }
}
