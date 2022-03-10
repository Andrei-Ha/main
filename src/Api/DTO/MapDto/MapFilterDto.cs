using System;

namespace Exadel.OfficeBooking.Api.DTO.MapDto
{
    public class MapFilterDto
    {
        public bool? IsKitchenPresent { get; set; }
        public bool? IsMeetingRoomPresent { get; set; }
        public Guid? OfficeId { get; set; }
    }
}
