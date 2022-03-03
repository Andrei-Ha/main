using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.Domain.OfficePlan;
using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class User : BaseModel
    {
        public long TelegramId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role{ get; set; }
        public DateTime? EmploymentStart { get; set; }
        public DateTime? EmploymentEnd { get; set; }
        public List<Vacation>? Vacations { get; set; }
        public List<Booking> BookingList { get; set; } = new();
        public Workplace? Preferred { get; set; }
    }
}
