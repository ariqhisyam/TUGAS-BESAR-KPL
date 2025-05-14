using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TUGASBESAR_kelompok_SagaraDailyCheckUp.Base;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{

    public class Kendaraan 
    {

        public int Id { get; set; }

        [JsonPropertyName("merek")]
        public string Merek { get; set; }
        [JsonPropertyName("platNomor")]
        public string PlatNomor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsReady { get; set; }
    }
}
