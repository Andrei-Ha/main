using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.Reports
{
    public class EmployeeReport
    {
        public string UserName { get; set; } = string.Empty;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<EmployeeDailyReport> EmployeeDailyReportList { get; set; } = new();
    }
}
