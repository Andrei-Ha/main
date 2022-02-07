using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.Models.Office;
using Exadel.OfficeBooking.Api.ViewModels;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
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
        public async Task<List<OfficeViewModel>> GetOffices(OfficeFilterModel filterModel)
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


            return await offices.Select<Office, OfficeViewModel>(s => s).ToListAsync();

        }

        
        public async Task<OfficeViewModel> GetOfficeById(Guid id)
        {
            return await _context.Offices.FirstOrDefaultAsync(f => f.Id == id);

        }
            

        public  async void DeleteOffice(Guid id)
        {
            var result =  _context.Offices.FirstOrDefault(f => f.Id == id);

            if (result != null)
            {
                 _context.Offices.Remove(result);

               await _context.SaveChangesAsync();
            }
        }

        public async Task<Guid> SaveOffice(OfficeViewModel officeVM)
        {
            Office office = new() 
            {
                Adress = officeVM.Adress,
                City = officeVM.City,
                Country = officeVM.Country,
                IsCityCenter = officeVM.IsCityCenter,
                IsParkingAvailable = officeVM.IsParkingAvailable,
                Name = officeVM.Name
            };

            _context.Offices.Add(office);
           await  _context.SaveChangesAsync();

            return office.Id;
        }
    }
}

