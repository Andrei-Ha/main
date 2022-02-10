using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.Bookings
{
    public class RecuringBooking : BaseModel
    {
        public List<Booking>? Bookings { get; set; }
    }
}
