using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using System;

namespace Exadel.OfficeBooking.Api.DTO.ReportDto
{
    public class EmployeeDailyReportDto
    {
        public DateTime CurrentDate { get; set; }
        public WorkplaceGetDto Workplace { get; set; } = new();
    }
}
