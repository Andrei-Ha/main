namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Parkingplace
    {
        public int PlaceNumber { get; set; }

        public bool IsBookingPossible { get; set; }

        public Office Office { get; set; } = new();

        public Booking Booking { get; set; } = new();
    }
}
