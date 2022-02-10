using System;

namespace Exadel.OfficeBooking.Domain.Reports
{
    public class DailyReport
    {
        public DateTime CurrentDate { get; set; }
        public int FreeWorkplaces { get; set; }
        public int TotalAmountOfWorkplaces { get; set; }
    }
}
