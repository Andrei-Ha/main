﻿using System;
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
    public string RecurringWeekDays { get; set; } = "0000000";
    public RecurringFrequency Frequency { get; set; }
}