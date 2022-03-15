using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Api.DTO.ReportDto
{
    public class CityReportDto
    {
        public string CityName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReportDto> CityDailyReportList { get; set; } = new();
    }
}
