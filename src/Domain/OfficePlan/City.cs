
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class City : BaseModel
    {
        public string? Name { get; set; }
        public int TimeZone { get; set; }
        public int SunnyDaysPerYear { get; set; }
        public string? CountryId { get; set; }
        public ICollection<Office>? Offices { get; set; }

    }
}
