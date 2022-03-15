using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Api.DTO.ReportDto
{
    public class EmployeeReportDto
    {
        public string UserName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<EmployeeDailyReportDto> EmployeeDailyReportList { get; set; } = new();
    }
}
