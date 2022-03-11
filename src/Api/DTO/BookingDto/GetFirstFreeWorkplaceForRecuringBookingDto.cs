using Exadel.OfficeBooking.Domain.Bookings;
using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto
{
    public class GetFirstFreeWorkplaceForRecuringBookingDto
    {
        public Guid UserId { get; set; }
        public Guid OfficeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? Count { get; set; }
        public int Interval { get; set; } = 1;
        public WeekDays RecurringWeekDays { get; set; }
        public RecurringFrequency Frequency { get; set; }
    }
}
