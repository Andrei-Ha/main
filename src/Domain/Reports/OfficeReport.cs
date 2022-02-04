namespace Exadel.OfficeBooking.Domain.Reports
{
    public class OfficeReport
    {
        public string OfficeName { get; set; } = string.Empty;

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set;}

        public List<DailyReport> OfficeDailyReportList { get; set; } = new();
    }
}
