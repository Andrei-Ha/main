

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Office : BaseModel
    {
        public string? Name { get; set; }
        public bool IsCityCenter { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public List<Map> maps { get; set; } = new();
        public List<ParkingPlace> ParkingPlaces { get; set; }
    }
}
