using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Office : BaseModel
    {
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsFreeParkingAvailable { get; set; }

        public List<Map>? Maps { get; set; }

        public List<ParkingPlace>? ParkingPlaces { get; set; }
    }
}
