using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.MapDto
{
    public class MapGetDto : MapSetDto
    {
        public Guid Id { get; set; }
        public string GetNameWithAttributes(bool isForButton = false)
        {
            string name = FloorNumber.ToString();
            if (isForButton)
            {
                name += "  (";
                name += IsKitchenPresent ? "🍽" : " __ ,";
                name += IsMeetingRoomPresent ? "🚪" : " __ ";
                return name + ")";
            }
            else
            {
                name += "  ( ";
                name += IsKitchenPresent ? "🍽 " : string.Empty;
                name += IsMeetingRoomPresent ? "🚪 " : string.Empty;
                name += ")";
                name = IsKitchenPresent || IsMeetingRoomPresent ? name : FloorNumber.ToString();
            }
            return name;
        }
    }
}
