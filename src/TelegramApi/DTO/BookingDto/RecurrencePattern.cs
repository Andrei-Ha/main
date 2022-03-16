using System;
using Exadel.OfficeBooking.Domain.Bookings;

namespace Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;

public class RecurrencePattern
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int Count { get; set; } = 0;
    public int Interval { get; set; } = 1;
    public WeekDays RecurringWeekDays { get; set; }
    public RecurringFrequency Frequency { get; set; }
    public BookingTypeEnum BookingType { get; set; }

    public bool IsAllDataComplete()
    {
        return BookingType switch
        {
            BookingTypeEnum.OneDay => StartDate != default,
            BookingTypeEnum.Continuous => StartDate != default && EndDate != default,
            BookingTypeEnum.Recurring => (Frequency != RecurringFrequency.Weekly && StartDate != default && (EndDate != default || Count > 0))
            || (Frequency == RecurringFrequency.Weekly && RecurringWeekDays != WeekDays.None && StartDate != default && (EndDate != default || Count > 0)),
            _ => false
        };
    }
}
