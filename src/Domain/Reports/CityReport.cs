namespace Exadel.OfficeBooking.Domain.Reports
{
    public class CityReport
    {
        public string CityName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReport> CityDailyReportList { get; set; } = new();
    }
}
