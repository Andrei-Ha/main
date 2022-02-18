using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IMapService
    {
        Task<MapDto[]> GetMap();
        Task<MapDto?> GetMapById(Guid id);
        Task<MapDto> CreateMap(MapDto map);
        Task<MapDto?> UpdateMap(Guid id, MapDto map);
        Task<MapDto?> DeleteMap(Guid id);
    }
}
