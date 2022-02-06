using Exadel.OfficeBooking.Domain.Bookings;


namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Workplace : BaseModel
    {
        public bool IsBooked { get; set; }

        public bool IsNextToWindow { get; set; }

        public bool HasPC { get; set; }

        public bool HasMonitor { get; set; }

        public bool HasKeyboard { get; set; }

        public bool HasMouse { get; set; }

        public bool HasHeadset { get; set; }

        public Map Map { get; set; } = new();

        public List<Booking>? Bookings { get; set; }
    }
}
