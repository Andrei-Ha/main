

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Office : BaseModel
    {
        public string? Name { get; set; }
        public bool IsCityCenter { get; set; }
        
        public string? CityId { get; set; }
        public ICollection<Floor>? Floors { get; set; }
        public ICollection<ParkingPlace>? ParkingPlaces { get; set; }
    }
}
