using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;

namespace Exadel.OfficeBooking.Domain
{
    public class Booking : BaseModel
    {
        public string RecurringDays { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public User User { get; set; } = new();
        public Workplace WorkSpace { get; set; } = new();
        public Parkingplace Parkingplace { get; set; } = new();
    }
}
