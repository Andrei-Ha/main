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
            if (IsKitchenPresent == true)
            {
                stringBuilder.Append($"{nameof(IsKitchenPresent)}=true&");
            }

            if (IsMeetingRoomPresent == true)
            {
                stringBuilder.Append($"{nameof(IsMeetingRoomPresent)}=true&");
            }

            if (OfficeId != null)
            {
                stringBuilder.Append($"{nameof(OfficeId)}={OfficeId}&");
            }

            return stringBuilder.ToString();
        }
    }
}
