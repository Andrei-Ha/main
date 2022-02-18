using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.MapDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.Api.Services
{
    public class MapService : IMapService
    {
        private readonly AppDbContext _context;

        public MapService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MapGetDto[]> GetMaps()
        {
            var maps = await _context.Maps.AsNoTracking().ToArrayAsync();
            return maps.Adapt<MapGetDto[]>();
        }

        public async Task<MapGetDto?> GetMapById(Guid id)
        {
            var map = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (map == null)
                return null;

            return map.Adapt<MapGetDto>();
        }

        public async Task<MapGetDto> CreateMap(MapSetDto map)
        {
            var mapDomain = map.Adapt<Map>();

            await _context.Maps.AddAsync(mapDomain);
            await _context.SaveChangesAsync();

            return mapDomain.Adapt<MapGetDto>();
        }

        public async Task<MapGetDto?> UpdateMap(Guid id, MapSetDto mapDto)
        {
            var mapFromDb = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (mapFromDb == null)
                return null;

            var mapDomain = mapDto.Adapt<Map>();

            mapDomain.Id = id;

            _context.Maps.Update(mapDomain);
            await _context.SaveChangesAsync();

            return mapDomain.Adapt<MapGetDto>();
        }

        public async Task<MapGetDto?> DeleteMap(Guid id)
        {
            var mapFromDb = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (mapFromDb == null)
                return null;

            _context.Maps.Remove(mapFromDb);
            await _context.SaveChangesAsync();

            return mapFromDb.Adapt<MapGetDto>();
        }
    }
}
