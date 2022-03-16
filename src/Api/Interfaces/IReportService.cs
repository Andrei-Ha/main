using Exadel.OfficeBooking.Api.DTO.ReportDto;
using System;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IReportService
    {
        public Task<OfficeReportDto?> GetOfficeReportByIdFromDateToDate(Guid id, DateTime fromDate, DateTime toDate);
    }
}
