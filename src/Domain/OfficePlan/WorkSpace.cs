

namespace Exadel.OfficeBooking.Domain
{
    public class WorkSpace : BaseModel
    {
       public bool BookPossibility { get; set; }
       public bool IsNearTheDoor { get; set; }
       public bool IsNearTheWindow { get; set; }
        public bool IsDeskTop { get; set; }
        public bool IsLaptop { get; set; }
        public string? RoomId { get; set; }
    }
}
