using System;
using System.Linq;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.Api.Services
{
    public class OfficeService : IOfficeService
    {
        private readonly AppDbContext _context;

        public OfficeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OfficeGetDto[]> GetOffices()
        {
            var offices = await _context.Offices.AsNoTracking().ToArrayAsync();
            return offices.Adapt<OfficeGetDto[]>();
        }

        public async Task<OfficeGetDto?> GetOfficeById(Guid id)
        {
            var office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            if (office == null) return null;

            return office.Adapt<OfficeGetDto>();
        }

        public async Task<OfficeGetDto> CreateOffice(OfficeSetDto office)
        {
            var officeDomain = office.Adapt<Office>();
            _context.Offices.Add(officeDomain);
            await _context.SaveChangesAsync();

            return officeDomain.Adapt<OfficeGetDto>();
        }

        public async Task<OfficeGetDto?> UpdateOffice(Guid id, OfficeGetDto officeGetDto)
        {
            Office? office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(o =>o.Id == id);
            if (office == null)
            {
                return null;
            }
            var officeUpd = officeGetDto.Adapt<Office>();
            officeUpd.Id = id;
            _context.Offices.Update(officeUpd);
            await _context.SaveChangesAsync();

            return officeUpd.Adapt<OfficeGetDto>();
        }

        public async Task<OfficeGetDto?> DeleteOffice(Guid id)
        {
            var result = _context.Offices.FirstOrDefault(f => f.Id == id);

            if (result == null) return null;

            _context.Offices.Remove(result);
            await _context.SaveChangesAsync();

            return result.Adapt<OfficeGetDto>();
        }
    }
}
