using Exadel.OfficeBooking.Domain.OfficePlan;

namespace Exadel.OfficeBooking.Domain.Reports
{
    public class EmployeeDailyReport
    {
        public DateTime CurrentDate { get; set; }
        public Workplace Workplace { get; set; } = new();
    }
}
