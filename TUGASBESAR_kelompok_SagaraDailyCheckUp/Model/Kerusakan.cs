using System.Text.Json.Serialization;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{
    public class Kerusakan
    {
        [JsonPropertyName("merek")]
        public string Merek { get; set; }
        [JsonPropertyName("platNomor")]
        public string PlatNomor { get; set; }
        [JsonPropertyName("kendala")]
        public string Kendala { get; set; }
        [JsonPropertyName("Catatan")]
        public string Catatan { get; set; }
    }
}
