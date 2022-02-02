

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Country : BaseModel
    {

        public string? Name { get; set; }

        public bool IsSea { get; set; }

        public ICollection<City>? Cities { get; set; }

    }
}
