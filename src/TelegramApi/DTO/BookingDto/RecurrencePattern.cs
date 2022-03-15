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
}
