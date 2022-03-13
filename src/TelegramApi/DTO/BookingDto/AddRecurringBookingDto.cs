using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;

public class AddRecurringBookingDto
{
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? Count { get; set; }
    public int Interval { get; set; } = 1;
    public WeekDays RecurringWeekDays { get; set; }
    public RecurringFrequency Frequency { get; set; }
}
