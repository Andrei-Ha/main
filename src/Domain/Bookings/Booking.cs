using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;
using System;

namespace Exadel.OfficeBooking.Domain.Bookings
{
    public class Booking : BaseModel
    {
        public User User { get; set; } = null!;
        
        public Workplace Workplace { get; set; } = null!;

        public ParkingPlace? ParkingPlace { get; set; }
        public Guid? ParkingPlaceId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsRecurring { get; set; }
        
        public int? Count { get; set; }
        public int Interval { get; set; } = 1;
        public RecurringFrequency Frequency { get; set; }
        public WeekDays RecurringWeekDays { get; set; }
    }
}
