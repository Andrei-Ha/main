
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Floor : BaseModel
    {
        public int Number { get; set; }
        public bool IsFirts { get; set; }
        public bool IsLast { get; set; }
        public string? OfficeId { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
