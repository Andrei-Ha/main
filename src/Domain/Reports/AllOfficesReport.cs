using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.Domain.Reports
{
    public class AllOfficesReport
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<DailyReport> AllOfficesDailyReportList { get; set; } = new();
    }
}
