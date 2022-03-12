using System;
using System.Text;

namespace Exadel.OfficeBooking.TelegramApi.DTO.MapDto
{
    public class MapFilterDto
    {
        public bool? IsKitchenPresent { get; set; }
        public bool? IsMeetingRoomPresent { get; set; }
        public Guid? OfficeId { get; set; }

        public string GetQueryString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(IsKitchenPresent == true ? $"{nameof(IsKitchenPresent)}=true&" : string.Empty);
            stringBuilder.Append(IsMeetingRoomPresent == true ? $"{nameof(IsMeetingRoomPresent)}=true&" : string.Empty);
            stringBuilder.Append(OfficeId != null ? $"{nameof(OfficeId)}={OfficeId}&" : string.Empty);
            return stringBuilder.ToString();
        }
    }
}
