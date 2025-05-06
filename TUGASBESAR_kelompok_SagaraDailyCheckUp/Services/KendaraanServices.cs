using TUGASBESAR_kelompok_SagaraDailyCheckUp.Model;

namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Services
{
    public class KendaraanService
    {
        public List<Kendaraan> GetKendaraanReady(List<Kendaraan> data)
        {
            return data.Where(k => k.IsReady).ToList();
        }
    }
}
