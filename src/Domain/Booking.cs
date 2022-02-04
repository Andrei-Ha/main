using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.Domain.Person;

namespace Exadel.OfficeBooking.Domain
{
    //Predefined microsoft enum
    //https://docs.microsoft.com/en-us/dotnet/api/system.dayofweek?view=net-6.0
    //
    //public enum DayOfWeek
    //{
    //    Sunday = 0,
    //    Monday = 1,
    //    Tuesday = 2,
    //    Wednesday = 3,
    //    Thursday = 4,
    //    Friday = 5,
    //    Saturday = 6
    //}

    public class Booking : BaseModel
    {
        public string RecurringDays { get; set; } = string.Empty;

        public bool[] WeeklyPattern = new bool[7];

        public bool[] MonthlyPattern = new bool[31];

        public bool IsRecurringWeekly { get; set; }

        public bool IsRecurringMonthly { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public User User { get; set; } = new();
        public Workplace WorkSpace { get; set; } = new();
        public Parkingplace Parkingplace { get; set; } = new();
    }
}
