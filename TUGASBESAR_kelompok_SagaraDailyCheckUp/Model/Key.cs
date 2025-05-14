using System.Text.Json.Serialization;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{
    public class Key
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("Keyvalue")]
        public string KeyValue { get; set; }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Driver = "Driver";
    }
}
