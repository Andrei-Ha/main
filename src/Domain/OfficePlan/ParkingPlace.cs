using Exadel.OfficeBooking.Domain.Bookings;
using System;

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class ParkingPlace : BaseModel
    {
        public bool IsBooked { get; set; }

        public int PlaceNumber { get; set; }
        public Guid OfficeId { get; set; }

        public virtual Office? Office { get; set; }
        
        public Booking? Booking { get; set; }
    }
}
