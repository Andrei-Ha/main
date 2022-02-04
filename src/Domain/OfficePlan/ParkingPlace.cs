using Exadel.OfficeBooking.Domain.Bookings;

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class ParkingPlace
    {
        public bool IsBooked { get; set; }

        public int PlaceNumber { get; set; }

        public Office Office { get; set; } = new();

        public Booking Booking { get; set; } = new();
    }
}
