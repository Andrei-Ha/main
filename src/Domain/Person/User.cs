using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class User
    {
        public Guid Id { get; set; }
        public int TelegramId { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public UserRole Role{ get; set; }
        // The use of this property needs to be clarified!
        //public int Position { get; set; }
        public DateOnly EmploymentStart { get; set; }
        public DateOnly? EmploymentEnd { get; set; }
        public List<Vacation> Vacations { get; set; } = new();
        //public IEnumerable<Booking> BookingList { get; set; }
        //TBD Seat ID. User can set prefferred seat
        public string PrefferedSeat { get; set; } = String.Empty;
    }
}
