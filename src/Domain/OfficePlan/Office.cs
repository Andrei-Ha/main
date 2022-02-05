namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Office : BaseModel
    {
        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Adress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool IsCityCenter { get; set; }

        public bool IsParkingAvailable { get; set; }

        public List<Map> Maps { get; set; } = new();

        public List<ParkingPlace>? ParkingPlaces { get; set; }
    }
}
