using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.MapDto
{
    public class MapSetDto
    {
        public int FloorNumber { get; set; }
        public bool IsKitchenPresent { get; set; }
        public bool IsMeetingRoomPresent { get; set; }
        public Guid OfficeId { get; set; }
    }
}
