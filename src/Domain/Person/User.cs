using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class User : BaseModel
    {
        public int TelegramId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role{ get; set; }
        public DateTime? EmploymentStart { get; set; }
        public DateTime? EmploymentEnd { get; set; }
        public List<Vacation>? Vacations { get; set; }
        public List<Booking>? BookingList { get; set; }
        public List<RecuringBooking>? RecuringBookingList { get; set; }
        public Workplace? Preferred { get; set; }
    }
}
