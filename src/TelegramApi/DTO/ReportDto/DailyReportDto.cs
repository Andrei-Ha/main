using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.ReportDto
{
    public class DailyReportDto
    {
        public DateTime CurrentDate { get; set; }
        public int FreeWorkplaces { get; set; }
        public int TotalAmountOfWorkplaces { get; set; }
    }
}
