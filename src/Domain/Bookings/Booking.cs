using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;

namespace Exadel.OfficeBooking.Domain.Bookings
{
    public class Booking : BaseModel
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public User User { get; set; } = new();
        public Workplace WorkSpace { get; set; } = new();
        public ParkingPlace ParkingPlace { get; set; } = new();
    }
}
