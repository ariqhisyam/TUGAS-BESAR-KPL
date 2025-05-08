namespace TUGASBESAR_kelompok_SagaraDailyCheckUp.Model
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNumber { get; set; }
        public string LicensePlate { get; set; }
        public string Status { get; set; } // Active, Inactive, etc.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Constructor
        public Driver()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }

}
