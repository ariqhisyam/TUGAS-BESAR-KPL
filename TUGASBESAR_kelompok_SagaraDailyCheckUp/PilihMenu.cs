namespace TUGASBESAR_kelompok_SagaraDailyCheckUp
{
    public class PilihMenu
    {
       
        public static void PilihMenu1()
        { //PUNYA FARRAS
            AdminLibrary.Adminlib admin = new AdminLibrary.Adminlib();
            Console.Clear();
            admin.salam("User");
            Console.WriteLine("Pilih menu:");
            Console.WriteLine("1. Menu Admin");
            Console.WriteLine("2. Menu Driver");
            Console.WriteLine("3. Menu Teknisi");
            Console.WriteLine("4. Keluar");
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
                    var menuTeknisi = new MenuTeknisi(); 
                    menuTeknisi.ShowMenuTeknisi().Wait(); 
                    break;
                case "4":
                    Console.WriteLine("Keluar...");
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }
        }
    }
}
