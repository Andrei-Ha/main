using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Services
{
    public class WorkplaceService : IWorkplaceService
    {
        private readonly AppDbContext _context;

        public WorkplaceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkplaceGetDto[]> GetWorkplaces()
        {
            var workplaces = await _context.Workplaces.AsNoTracking().ToArrayAsync();
            return workplaces.Adapt<WorkplaceGetDto[]>();
        }

        public async Task<WorkplaceGetDto[]> GetWorkplaces(WorkplaceFilterDto filterModel)
        {
            var workplaces = _context.Workplaces.AsNoTracking();

            if (!string.IsNullOrEmpty(filterModel.Number))
                workplaces = workplaces.Where(w => w.Number.Contains(filterModel.Number));

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

        public async Task<WorkplaceGetDto?> CreateWorkplace(WorkplaceSetDto workplace)
        {
            if (workplace == null)
                return null;

            var workplaceDomain = workplace.Adapt<Workplace>();

            await _context.Workplaces.AddAsync(workplaceDomain);
            await _context.SaveChangesAsync();

            return workplaceDomain.Adapt<WorkplaceGetDto>();
        }

        public async Task<WorkplaceGetDto?> UpdateWorkplace(WorkplaceGetDto workplaceDto)
        {
            if (workplaceDto == null)
                return null;

            var workplaceFromDb = await _context.Workplaces.AsNoTracking().FirstOrDefaultAsync(w => w.Id == workplaceDto.Id);

            if (workplaceFromDb == null)
                return null;

            var workplaceDomain = workplaceDto.Adapt<Workplace>();

            _context.Workplaces.Update(workplaceDomain);
            await _context.SaveChangesAsync();

            return workplaceDomain.Adapt<WorkplaceGetDto>();
        }

        public async Task<WorkplaceGetDto?> DeleteWorkplaceById(Guid id)
        {
            var workplaceFromDb = await _context.Workplaces.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);

            if (workplaceFromDb == null)
                return null;

            _context.Remove(workplaceFromDb);
            await _context.SaveChangesAsync();

            return workplaceFromDb.Adapt<WorkplaceGetDto>();
        }
    }
}
