using System;

namespace Exadel.OfficeBooking.Api.DTO
{
    public class MapDto
    {
        public Guid Id { get; set; }
        public bool? IsKitchenPresent { get; set; }
        public bool? IsMeetingRoomPresent { get; set; }
        public Guid OfficeId { get; set; }

    }
}
