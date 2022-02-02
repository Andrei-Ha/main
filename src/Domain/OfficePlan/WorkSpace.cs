

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class WorkSpace : BaseModel
    {
        public bool IsBookingPossible { get; set; }
        public bool IsNearTheDoor { get; set; }
        public bool IsNearTheWindow { get; set; }
        public bool IsDeskTop { get; set; }
        public bool IsLaptop { get; set; }
        public Room room { get; set; } = new();
        public string RoomId { get; set; }
        
    }
}
