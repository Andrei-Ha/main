using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IParkingPlaceService
    {
        Task<ParkingPlaceGetDto[]> GetParkings();

        Task<ParkingPlaceGetDto?> GetParkingById(Guid id);

        Task<ParkingPlaceGetDto> CreateParking(ParkingPlaceSetDto parking);

        Task<ParkingPlaceGetDto?> UpdateParking(Guid id, ParkingPlaceSetDto parking);

        Task<ParkingPlaceGetDto?> DeleteParking(Guid id);

    }
}
