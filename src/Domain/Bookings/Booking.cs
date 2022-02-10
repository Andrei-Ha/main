using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;
using System;

namespace Exadel.OfficeBooking.Domain.Bookings
{
    public class Booking : BaseModel
    {
        public DateTime BookingDate { get; set; }

        public User User { get; set; } = new();
        
        public Workplace Workplace { get; set; } = new();
        
        public ParkingPlace? ParkingPlace { get; set; }

        public Guid? ParkingPlaceId { get; set; }
    }
}
