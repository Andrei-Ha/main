using Exadel.OfficeBooking.Api.DTO.BookingDto;
using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Services
{
    public class WorkplaceService : IWorkplaceService
    {
        private readonly AppDbContext _context;
        private readonly IBookingService _bookingService;

        public WorkplaceService(AppDbContext context, IBookingService bookingService)
        {
            _context = context;
            _bookingService = bookingService;
        }

        public async Task<WorkplaceGetDto[]> GetWorkplaces()
        {
            var workplaces = await _context.Workplaces.AsNoTracking().ToArrayAsync();
            return workplaces.Adapt<WorkplaceGetDto[]>();
        }

        public async Task<WorkplaceGetDto[]> GetWorkplaces(WorkplaceFilterDto filterModel, Guid? officeId)
        {
            var workplaces = _context.Workplaces
                .Include(w => w.Map)
                .Include(w => w.Bookings)
                .AsNoTracking();

            if (officeId != null)
                workplaces = workplaces.Where(x => x.Map.OfficeId == officeId);

            if (filterModel.BookingType == BookingTypeEnum.OneDay)
            {
                workplaces = workplaces.Where(w => _bookingService
                .IsWorkplaceAvailableForOneDayBooking(w, (DateTime)filterModel.StartDate));
            }

            if (filterModel.BookingType == BookingTypeEnum.Continous ||
                filterModel.BookingType == BookingTypeEnum.Recuring)
            {
                List<DateTime> recurringDates = _bookingService.GetRecurringBookingDates(filterModel.Adapt<RecurrencePattern>());
                
                workplaces = workplaces.Where(w => _bookingService
                .IsWorkplaceAvailableForRecurringBooking(w, recurringDates));
            }

            if (!string.IsNullOrEmpty(filterModel.Name))
                workplaces = workplaces.Where(w => w.Name.Contains(filterModel.Name));

            if (filterModel.MapId != null)
                workplaces = workplaces.Where(w => w.MapId == filterModel.MapId);

            if (filterModel.Type != null)
                workplaces = workplaces.Where(w => w.Type.ToString() == filterModel.Type.ToString());

            if (filterModel.IsBooked != null)
                workplaces = workplaces.Where(w => w.IsBooked == filterModel.IsBooked);

            if (filterModel.IsNextToWindow != null)
                workplaces = workplaces.Where(w => w.IsNextToWindow == filterModel.IsNextToWindow);

            if (filterModel.HasPC != null)
                workplaces = workplaces.Where(w => w.HasPC == filterModel.HasPC);

            if (filterModel.HasMonitor != null)
                workplaces = workplaces.Where(w => w.HasMonitor == filterModel.HasMonitor);

            if (filterModel.HasKeyboard != null)
                workplaces = workplaces.Where(w => w.HasKeyboard == filterModel.HasKeyboard);

            if (filterModel.HasMouse != null)
                workplaces = workplaces.Where(w => w.HasMouse == filterModel.HasMouse);

            if (filterModel.HasHeadset != null)
                workplaces = workplaces.Where(w => w.HasHeadset == filterModel.HasHeadset);

            var workplacesList = await workplaces.ToArrayAsync();

            return workplacesList.Adapt<WorkplaceGetDto[]>();
        }

        public async Task<WorkplaceGetDto?> GetWorkplaceById(Guid id)
        {
            var workplace = await _context.Workplaces.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

            if (workplace == null)
                return null;

            return workplace.Adapt<WorkplaceGetDto>();
        }

        public async Task<WorkplaceGetDto> CreateWorkplace(WorkplaceSetDto workplace)
        {
            var workplaceDomain = workplace.Adapt<Workplace>();
            await _context.Workplaces.AddAsync(workplaceDomain);
            await _context.SaveChangesAsync();
            //Why in the created record the MapId does not match the sent one???
            //May be we need to get all Map with sent MapId and then save...
            return workplaceDomain.Adapt<WorkplaceGetDto>();
        }

        public async Task<WorkplaceGetDto?> UpdateWorkplace(Guid id, WorkplaceSetDto workplaceDto)
        {
            var workplaceFromDb = await _context.Workplaces.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

            if (workplaceFromDb == null)
                return null;

            var workplaceDomain = workplaceDto.Adapt<Workplace>();

            workplaceDomain.Id = id;

            _context.Workplaces.Update(workplaceDomain);
            await _context.SaveChangesAsync();

            return workplaceDomain.Adapt<WorkplaceGetDto>();
        }

        public async Task<WorkplaceGetDto?> DeleteWorkplaceById(Guid id)
        {
            var workplaceFromDb = await _context.Workplaces.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

            if (workplaceFromDb == null)
                return null;

            _context.Workplaces.Remove(workplaceFromDb);
            await _context.SaveChangesAsync();

            return workplaceFromDb.Adapt<WorkplaceGetDto>();
        }
    }
}
