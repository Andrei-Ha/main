namespace Exadel.OfficeBooking.Domain.Reports
{
    public class AllOfficesReport
    {
        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public List<DailyReport> AllOfficesDailyReportList { get; set; } = new();
    }
}
