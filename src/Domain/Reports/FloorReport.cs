namespace Exadel.OfficeBooking.Domain.Reports
{
    public class FloorReport
    {
        public string OfficeName { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReport> FloorDailyReportList { get; set; } = new();
    }
}
