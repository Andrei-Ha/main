
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class ParkingPlace
    {

        public int Number { get; set; }

        public bool IsBookingPossible { get; set; }
        public List<Office> office { get; set; } = new();

        public string? OfficeId { get; set; }

    }
}

