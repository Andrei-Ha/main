using System;
using Exadel.OfficeBooking.Domain.Bookings;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class GetBookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }
    public Guid? ParkingPlaceId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsRecurring { get; set; }

    public int? Count { get; set; }
    public int Interval { get; set; } = 1;
    public WeekDays RecurringWeekDays { get; set; }
    public RecurringFrequency Frequency { get; set; }

    public BookingTypeEnum BookingType { get; set; }

    public string WorkplaceName { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public string OfficeName { get; set; } = string.Empty;
}