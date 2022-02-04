using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain.Reports
{
    public class EmployeeDailyReport
    {
        public DateOnly CurrentDate { get; set; }
        public Workplace Workplace { get; set; } = new();
    }
}
