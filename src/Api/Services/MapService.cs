using System;
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
    public class MapService : IMapService
    {
        private readonly AppDbContext _context;

        public MapService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MapDto[]> GetMap()
        {
            var maps = await _context.Maps.AsNoTracking().ToArrayAsync();
            return maps.Adapt<MapDto[]>();
        }

        public async Task<MapDto?> GetMapById(Guid id)
        {
            var map = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            if (map == null) return null;

            return map.Adapt<MapDto>();
        }

        public async Task<MapDto> CreateMap(MapDto map)
        {
            var mapDomain = map.Adapt<Map>();
            _context.Maps.Add(mapDomain);
            await _context.SaveChangesAsync();

            return mapDomain.Adapt<MapDto>();
        }

        public async Task<MapDto?> UpdateMap(Guid id, MapDto mapDto)
        {
            Map? map = await _context.Maps.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
            if (map == null)
            {
                return null;
            }
            var mapUpd = mapDto.Adapt<Map>();
            mapUpd.Id = id;
            _context.Maps.Update(mapUpd);
            await _context.SaveChangesAsync();

            return mapUpd.Adapt<MapDto>();
        }

        public async Task<MapDto?> DeleteMap(Guid id)
        {
            var result = _context.Maps.FirstOrDefault(f => f.Id == id);

            if (result == null) return null;

            _context.Maps.Remove(result);
            await _context.SaveChangesAsync();

            return result.Adapt<MapDto>();
        }
    }
}
