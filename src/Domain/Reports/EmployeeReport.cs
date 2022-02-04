namespace Exadel.OfficeBooking.Domain.Reports
{
    public class EmployeeReport
    {
        public string UserName { get; set; } = string.Empty;

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public List<EmployeeDailyReport> EmployeeDailyReportList { get; set; } = new();
    }
}
