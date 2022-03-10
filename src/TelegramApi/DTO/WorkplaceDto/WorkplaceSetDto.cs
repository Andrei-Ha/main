using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto
{
    public class WorkplaceSetDto
    {
        public string Name { get; set; } = string.Empty;

        public WorkplaceTypesDto Type { get; set; }

        public bool IsBooked { get; set; }

        public bool IsNextToWindow { get; set; }

        public bool HasPC { get; set; }

        public bool HasMonitor { get; set; }

        public bool HasKeyboard { get; set; }

        public bool HasMouse { get; set; }

        public bool HasHeadset { get; set; }

        public Guid MapId { get; set; }
    }
}
