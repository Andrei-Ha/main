using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Api.DTO.ReportDto
{
    public class FloorReportDto
    {
        public string OfficeName { get; set; } = string.Empty;

        public int FloorNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReportDto> FloorDailyReportList { get; set; } = new();
    }
}
