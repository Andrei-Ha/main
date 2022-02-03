
namespace Exadel.OfficeBooking.Domain.OfficePlan
{
    public class Map : BaseModel
    {
        public int FloorNumber { get; set; }

        public bool IsKitchenPresent { get; set; }

        public bool IsMeetingRoomPresent { get; set; }

        public Office Office { get; set; } = new();

        public List<WorkSpace> Workspaces { get; set; } = new();
    }
}
