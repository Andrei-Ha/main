using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.MapDto
{
    public class MapGetDto : MapSetDto
    {
        public Guid Id { get; set; }
        public string GetNameWithAttributes()
        {
            string name = FloorNumber.ToString() +"  (";
            name += IsKitchenPresent ? "🍽" : " __ ,";
            name += IsMeetingRoomPresent ? "🚪" : " __ ";
            return name + ")";
        }
    }
}
