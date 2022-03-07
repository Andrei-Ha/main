using Exadel.OfficeBooking.Api.DTO.OfficeDto;
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

    public class ParkingPlaceService : IParkingPlaceService
    {
        private readonly AppDbContext _context;

        public ParkingPlaceService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ParkingPlaceGetDto[]> GetParkings()
        {
            var parkings = await _context.ParkingPlaces.AsNoTracking().ToArrayAsync();
            return parkings.Adapt<ParkingPlaceGetDto[]>();
        }

        public async Task<ParkingPlaceGetDto?> GetParkingById(Guid id)
        {
            var parking = await _context.ParkingPlaces.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (parking == null)
                return null;

            return parking.Adapt<ParkingPlaceGetDto>();
        }

        public async Task<ParkingPlaceGetDto?> CreateParking(ParkingPlaceSetDto parking)
        {
            var ParkingPlaceDomain = parking.Adapt<ParkingPlace>();

            await _context.ParkingPlaces.AddAsync(ParkingPlaceDomain);
            await _context.SaveChangesAsync();

            return ParkingPlaceDomain.Adapt<ParkingPlaceGetDto>();
        }

        public async Task<ParkingPlaceGetDto?> UpdateParking(Guid id, ParkingPlaceSetDto parking)
        {
            var ParkingFromDb = await _context.ParkingPlaces.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (ParkingFromDb == null)
                return null;

            var parkingPlaceDomain = parking.Adapt<ParkingPlace>();

            parkingPlaceDomain.Id = id;

            _context.ParkingPlaces.Update(parkingPlaceDomain);
            await _context.SaveChangesAsync();

            return parkingPlaceDomain.Adapt<ParkingPlaceGetDto>();
        }

        public async Task<ParkingPlaceGetDto?> DeleteParking(Guid id)
        {
            var parkingFromDb = await _context.ParkingPlaces.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (parkingFromDb == null)
                return null;

            _context.ParkingPlaces.Remove(parkingFromDb);
            await _context.SaveChangesAsync();

            return parkingFromDb.Adapt<ParkingPlaceGetDto>();
        }


    }
}











