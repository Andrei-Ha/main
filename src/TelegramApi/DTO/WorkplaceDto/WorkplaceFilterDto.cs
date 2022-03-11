using System;
using System.Text;

namespace Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto
{
    public class WorkplaceFilterDto
    {
        public string? Name { get; set; }

        public Guid? MapId { get; set; }

        public WorkplaceTypesDto? Type { get; set; }

        public bool? IsBooked { get; set; }

        public bool? IsNextToWindow { get; set; }

        public bool? IsVIP { get; set; }

        public bool? HasPC { get; set; }

        public bool? HasMonitor { get; set; }

        public bool? HasKeyboard { get; set; }

        public bool? HasMouse { get; set; }

        public bool? HasHeadset { get; set; }

        public string GetQueryString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (IsNextToWindow == true)
            {
                stringBuilder.Append($"{nameof(IsNextToWindow)}=true&");
            }

            if (IsVIP == true)
            {
                stringBuilder.Append($"{nameof(IsVIP)}=true&");
            }

            if (HasPC == true)
            {
                stringBuilder.Append($"{nameof(HasPC)}=true&");
            }

            if (HasMonitor == true)
            {
                stringBuilder.Append($"{nameof(HasMonitor)}=true&");
            }

            if (HasKeyboard == true)
            {
                stringBuilder.Append($"{nameof(HasKeyboard)}=true&");
            }

            if (HasMouse == true)
            {
                stringBuilder.Append($"{nameof(HasMouse)}=true&");
            }

            if (HasHeadset == true)
            {
                stringBuilder.Append($"{nameof(HasHeadset)}=true&");
            }

            if (MapId != null)
            {
                stringBuilder.Append($"{nameof(MapId)}={MapId}&");
            }

            return stringBuilder.ToString();
        }
    }
}
