using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.DTO.ReportDto
{
    public class OfficeReportDto
    {
        public string OfficeName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReportDto> OfficeDailyReportList { get; set; } = new();
    }
}
