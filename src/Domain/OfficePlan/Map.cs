
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Map : BaseModel
    {
        public int Number { get; set; }
        public bool IsFirts { get; set; }
        public bool IsLast { get; set; }
        public List<Office> office { get; set; } = new();
        public string? OfficeId { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
