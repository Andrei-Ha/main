
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class ParkingPlace
    {
        public int PlaceNumber { get; set; }

        public bool IsBookingPossible { get; set; }

        public Office Office { get; set; } = new();

    }
}

