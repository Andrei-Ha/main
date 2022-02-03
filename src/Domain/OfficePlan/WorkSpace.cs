

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class WorkSpace : BaseModel
    {
        public bool IsBookingPossible { get; set; }

        public bool IsNearTheDoor { get; set; }

        public bool IsNearTheWindow { get; set; }

        public bool IsDesktop { get; set; }

        public bool IsLaptop { get; set; }

        public Map Map { get; set; } = new();

    }
}
