using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using System;
using System.Text;

namespace Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto
{
    public class WorkplaceFilterDto
    {
        // out of UserState
        public string? Name { get; set; }

        public Guid? OfficeId { get; set; }

        public Guid? MapId { get; set; }

        // out of UserState
        public WorkplaceTypesDto? Type { get; set; }

        public bool? IsBooked { get; set; }

        public bool? IsNextToWindow { get; set; }

        public bool? HasPC { get; set; }

        public bool? HasMonitor { get; set; }

        public bool? HasKeyboard { get; set; }

        public bool? HasMouse { get; set; }

        public bool? HasHeadset { get; set; }

        //

        // if true, then only one instance is returned!
        public bool? IsOnlyFirstFree { get; set; }

        public BookingTypeEnum? BookingType { get; set; }

        //

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Count { get; set; }

        public int? Interval { get; set; } = 1;

        public WeekDays? RecurringWeekDays { get; set; }

        public RecurringFrequency? Frequency { get; set; }

        public string GetQueryString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(Name != null ? $"{nameof(Name)}={Name}&" : string.Empty);
            stringBuilder.Append(OfficeId != null ? $"{nameof(OfficeId)}={OfficeId}&" : string.Empty);
            stringBuilder.Append(MapId != null ? $"{nameof(MapId)}={MapId}&" : string.Empty);
            stringBuilder.Append(Type != null ? $"{nameof(Type)}={Type}&" : string.Empty);

            stringBuilder.Append(IsNextToWindow == true ? $"{nameof(IsNextToWindow)}=true&" : string.Empty);
            stringBuilder.Append(HasPC == true ? $"{nameof(HasPC)}=true&" : string.Empty);
            stringBuilder.Append(HasMonitor == true ? $"{nameof(HasMonitor)}=true&" : string.Empty);
            stringBuilder.Append(HasKeyboard == true ? $"{nameof(HasKeyboard)}=true&" : string.Empty);
            stringBuilder.Append(HasMouse == true ? $"{nameof(HasMouse)}=true&" : string.Empty);
            stringBuilder.Append(HasHeadset == true ? $"{nameof(HasHeadset)}=true&" : string.Empty);

            stringBuilder.Append(IsOnlyFirstFree == true ? $"{nameof(IsOnlyFirstFree)}=true&" : string.Empty);
            stringBuilder.Append(BookingType != null ? $"{nameof(BookingType)}={BookingType}&" : string.Empty);

            stringBuilder.Append(StartDate != null ? $"{nameof(StartDate)}={StartDate:yyyy-MM-dd}&" : string.Empty);
            stringBuilder.Append(EndDate != null ? $"{nameof(EndDate)}={EndDate:yyyy-MM-dd}&" : string.Empty);
            stringBuilder.Append(Count != null ? $"{nameof(Count)}={Count}&" : string.Empty);
            stringBuilder.Append(Interval != null ? $"{nameof(Interval)}={Interval}&" : string.Empty);
            stringBuilder.Append(RecurringWeekDays != null ? $"{nameof(RecurringWeekDays)}={RecurringWeekDays}&" : string.Empty);
            stringBuilder.Append(Frequency != null ? $"{nameof(Frequency)}={Frequency}" : string.Empty);

            return stringBuilder.ToString();
        }
    }
}
