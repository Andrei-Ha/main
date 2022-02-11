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

        public async Task<OfficeGetDto[]> GetOffices(OfficeFilterDto filterModel)
        {
            var offices = _context.Offices.AsNoTracking();

            if (!string.IsNullOrEmpty(filterModel.Name))
                offices = offices.Where(w => w.Name.Contains(filterModel.Name));

            if (!string.IsNullOrEmpty(filterModel.City))
                offices = offices.Where(w => w.City.Contains(filterModel.City));

            if (!string.IsNullOrEmpty(filterModel.Country))
                offices = offices.Where(w => w.Country.Contains(filterModel.Country));

            if (!string.IsNullOrEmpty(filterModel.Adress))
                offices = offices.Where(w => w.Adress.Contains(filterModel.Adress));

            if (filterModel.IsCityCenter != null)
                offices = offices.Where(w => w.IsCityCenter == filterModel.IsCityCenter);

            if (filterModel.IsParkingAvailable != null)
                offices = offices.Where(w => w.IsParkingAvailable == filterModel.IsParkingAvailable);

            var result = await offices.ToArrayAsync();

            return result.Adapt<OfficeGetDto[]>();
        }

        public async Task<OfficeGetDto?> GetOfficeById(Guid id)
        {
            var office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            if (office == null) return null;

            return office.Adapt<OfficeGetDto>();
        }

        public async Task<OfficeGetDto?> CreateOffice(OfficeSetDto office)
        {
            if (office == null) return null;

            var officeDomain = office.Adapt<Office>();

            await _context.Offices.AddAsync(officeDomain);
            await _context.SaveChangesAsync();

            return officeDomain.Adapt<OfficeGetDto>();
        }

        public async Task<OfficeGetDto?> UpdateOffice(OfficeGetDto office)
        {
            if (office == null) return null;

            var officeDomain = office.Adapt<Workplace>();

            _context.Entry(officeDomain).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return office;
        }

        public async Task<Guid?> DeleteOffice(Guid id)
        {
            var result = _context.Offices.FirstOrDefault(f => f.Id == id);

            if (result == null) return null;

            _context.Offices.Remove(result);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
