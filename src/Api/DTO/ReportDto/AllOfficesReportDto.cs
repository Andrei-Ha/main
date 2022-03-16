using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Api.DTO.ReportDto
{
    public class AllOfficesReportDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<OfficeReportDto> OfficesReportsList { get; set; } = new();
    }
}
