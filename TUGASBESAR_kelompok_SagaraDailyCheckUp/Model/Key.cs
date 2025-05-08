namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{
    public class Key
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string KeyValue { get; set; }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Driver = "Driver";
    }
}
