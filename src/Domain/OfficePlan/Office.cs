

using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain
{
    public class Office : BaseModel
    {
        public string? Name { get; set; }
        public bool IsCityCenter { get; set; }
        
        public string? CityId { get; set; }
        public ICollection<Floor> Floors { get; set; }
        public ICollection<ParkingPlace>? ParkingPlaces { get; set; }
    }
}
