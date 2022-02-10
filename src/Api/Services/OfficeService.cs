using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;
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
        public async Task<List<OfficeDto>> GetOffices(OfficeFilterDto filterModel)
        {
            var offices = _context.Offices.AsQueryable();

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


            var result = await offices.AsNoTracking().ToListAsync();

            return result.Adapt<List<OfficeDto>>();

        }


        public async Task<OfficeDto?> GetOfficeById(Guid id)
        {
            var office = await _context.Offices.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            if (office == null) return null;

            return office.Adapt<OfficeDto>();

        }


        public async Task<Guid?> DeleteOffice(Guid id)
        {
            var result = _context.Offices.FirstOrDefault(f => f.Id == id);

            if (result == null) return null;
            
                _context.Offices.Remove(result);

                await _context.SaveChangesAsync();

                return id;
            
        }

        public async Task<Guid?> SaveOffice(OfficeDto office)
        {
            var officeDomain = office.Adapt<Office>();

            await _context.Offices.AddAsync(officeDomain);
            await _context.SaveChangesAsync();

            return office.Id;
        }

        public async Task<OfficeDto?> UpdateOffice(OfficeDto office)
        {
            var officeDomain = office.Adapt<Workplace>();

            _context.Entry(officeDomain).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return office;
        }

    }
}

