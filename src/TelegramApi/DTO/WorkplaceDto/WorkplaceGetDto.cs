using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto
{
    public class WorkplaceGetDto : WorkplaceSetDto
    {
        public Guid Id { get; set; }
        public string GetNameWithAttributes()
        {
            string name = Name.ToString() + "  (";
            name += IsNextToWindow ? "🪟 " : " __ ,";
            name += HasPC ? "💻 " : " __ ";
            name += HasMonitor ? "🖥 " : " __ ,";
            name += HasKeyboard ? "⌨️ " : " __ ";
            name += HasMouse ? "🐭 " : " __ ,";
            name += HasHeadset ? "🎧 " : " __ ";
            return name + ")";
        }
    }
}
