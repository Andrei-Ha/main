

namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Room : BaseModel
    {
        public string? Name { get; set; }
        public bool IsSunnySide { get; set; }
        public List<Map> maps { get; set; } = new();
        public string? MapId { get; set; }
        
        public List<WorkSpace> Workspaces { get; set; }

    }
}
