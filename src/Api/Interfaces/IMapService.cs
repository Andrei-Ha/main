using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.MapDto;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IMapService
    {
        Task<MapGetDto[]> GetMaps();
        Task<MapGetDto?> GetMapById(Guid id);
        Task<MapGetDto> CreateMap(MapSetDto map);
        Task<MapGetDto?> UpdateMap(Guid id, MapSetDto map);
        Task<MapGetDto?> DeleteMap(Guid id);
    }
}
