using Exadel.OfficeBooking.Api.DTO.ReportDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<OfficeReportDto>> GetOfficeReportByIdFromDateToDate(Guid id, DateTime fromDate, DateTime toDate)
        {
            if (toDate <= fromDate)
                return BadRequest(new { message = "Wrong date range" });

            var officeReport = await _reportService.GetOfficeReportByIdFromDateToDate(id, fromDate, toDate);

            if (officeReport == null)
                return NotFound(new { message = "The requested office was not found" });

            return Ok(officeReport);
        }
    }
}
