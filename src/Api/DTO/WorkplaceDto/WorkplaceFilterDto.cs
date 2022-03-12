using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Domain.Bookings;
using System;

namespace Exadel.OfficeBooking.Api.DTO.WorkplaceDto
{
    public class WorkplaceFilterDto
    {
        // out of UserState
        public string? Name { get; set; }

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
        public bool? IsOnlyFirst { get; set; }

        public BookingTypeEnum BookingType { get; set; } = BookingTypeEnum.None;

        //

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Count { get; set; }

        public int? Interval { get; set; } = 1;

        public WeekDays? RecurringWeekDays { get; set; }

        public RecurringFrequency? Frequency { get; set; }
    }
}
