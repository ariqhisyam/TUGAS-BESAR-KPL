using TUGASBESAR_kelompok_SagaraDailyCheckUp.Base;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{

    public class Kendaraan : BaseEntity
    {
        public string Nama { get; set; }
        public string PlatNomor { get; set; }
        public bool IsReady { get; set; } // Added the missing property  
        public string Merek { get; internal set; }
    }
}
