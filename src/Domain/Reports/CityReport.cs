namespace Exadel.OfficeBooking.Domain.Reports
{
    public class CityReport
    {
        public string CityName { get; set; } = string.Empty;

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public List<DailyReport> CityDailyReportList { get; set; } = new();
    }
}
