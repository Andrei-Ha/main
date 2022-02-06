namespace Exadel.OfficeBooking.Domain.Reports
{
    public class OfficeReport
    {
        public string OfficeName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set;}

        public List<DailyReport> OfficeDailyReportList { get; set; } = new();
    }
}
