using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;

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
    public string Summary { get; set; } = string.Empty;

    public string WorkplaceName { get; set; } = string.Empty;
    public int FloorNumber { get; set; }
    public Guid OfficeId { get; set; } = default;
    public string OfficeName { get; set; } = string.Empty;
    public int? ParkingPlaceNumber { get; set; }

    public void MapDateIntoState(ref UserState state)
    {
        state.WorkplaceId = WorkplaceId;
        // ??? ParkingPlaceId
        state.StartDate = StartDate;
        state.EndDate = EndDate ?? default;
        state.Count = Count ?? 0;
        state.Interval = Interval;
        state.RecurringWeekDays = RecurringWeekDays;
        state.Frequency = Frequency;
        state.BookingType = BookingType;
        state.WorkplaceName = WorkplaceName;
        state.OfficeId = OfficeId;
        state.OfficeName = OfficeName;
        state.FloorName = FloorNumber.ToString();
        //parkingPlaceNumber?
    }
}