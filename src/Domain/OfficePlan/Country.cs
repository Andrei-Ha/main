
using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain
{
    public class Country : BaseModel
    {

        public string? Name { get; set; }

        public bool IsSea { get; set; }

        public ICollection<City> Cities { get; set; }

    }
}
