using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto
{
    public class WorkplaceGetDto : WorkplaceSetDto
    {
        public Guid Id { get; set; }
        public string GetNameWithAttributes(bool isForButton = false)
        {
            string name = Name.ToString();
            if (isForButton)
            {
                name += "  (";
                name += IsNextToWindow ? "🪟 " : " __ ,";
                name += HasPC ? "💻 " : " __ ";
                name += HasMonitor ? "🖥 " : " __ ,";
                name += HasKeyboard ? "⌨️ " : " __ ";
                name += HasMouse ? "🐭 " : " __ ,";
                name += HasHeadset ? "🎧 " : " __ ";
                name += ")";
            }
            else
            {
                name += "  ( ";
                name += IsNextToWindow ? "🪟 " : string.Empty;
                name += HasPC ? "💻 " : string.Empty;
                name += HasMonitor ? "🖥 " : string.Empty;
                name += HasKeyboard ? "⌨️ " : string.Empty;
                name += HasMouse ? "🐭 " : string.Empty;
                name += HasHeadset ? "🎧 " : string.Empty;
                name += ")";
                name = IsNextToWindow || HasPC || HasMonitor || HasKeyboard || HasMouse || HasHeadset ? name : Name.ToString();
            }
            return name;
        }
    }
}
