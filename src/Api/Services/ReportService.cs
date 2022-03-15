using Exadel.OfficeBooking.Api.DTO.ReportDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly IBookingService _bookingService;

        public ReportService(AppDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<OfficeReportDto?> GetOfficeReportByIdFromDateToDate(Guid id, DateTime fromDate, DateTime toDate)
        {
            var office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (office == null)
                return null;

            var officeMaps = await _context.Maps.AsNoTracking().Where(m => m.OfficeId == id).ToArrayAsync();

            List<DailyReportDto> officeDailyReportList = new();

            for (var dt = fromDate; dt <= toDate; dt = dt.AddDays(1))
            {
                int freeWorkplaces = 0;
                int totalAmountOfWorkplaces = 0;

                foreach (var officeMap in officeMaps)
                {
                    var workplaces = await _context.Workplaces.Include(w => w.Bookings)
                        .Where(w => w.MapId == officeMap.Id).ToListAsync();

                    totalAmountOfWorkplaces += workplaces.Count;

                    foreach (var workplace in workplaces)
                    {
                        if (_bookingService.IsWorkplaceAvailableForOneDayBooking(workplace, dt))
                            freeWorkplaces++;
                    }
                }

                officeDailyReportList.Add(new DailyReportDto
                {
                    CurrentDate = dt,
                    FreeWorkplaces = freeWorkplaces,
                    TotalAmountOfWorkplaces = totalAmountOfWorkplaces
                });
            }
            
            return new OfficeReportDto
            {
                OfficeName = office.Name,
                FromDate = fromDate,
                ToDate = toDate,
                OfficeDailyReportList = officeDailyReportList
            };
        }
    }
}
