namespace Exadel.OfficeBooking.Domain.Reports
{
    public class FloorReport
    {
        public string OfficeName { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public List<DailyReport> FloorDailyReportList { get; set; } = new();
    }
}
