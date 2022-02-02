

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Room : BaseModel
    {
        public string? Name { get; set; }
        public bool IsSunnySide { get; set; }
        public string? FloorId { get; set; }
        public ICollection<WorkSpace>? Workspaces { get; set; }

    }
}
