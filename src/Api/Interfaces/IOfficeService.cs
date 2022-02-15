using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.OfficeDto;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IOfficeService
    {
        Task<OfficeGetDto[]> GetOffices();

        Task<OfficeGetDto?> GetOfficeById(Guid id);

        Task<OfficeGetDto> CreateOffice(OfficeSetDto office);

        Task<OfficeGetDto?> UpdateOffice(Guid id, OfficeGetDto office);

        Task<OfficeGetDto?> DeleteOffice(Guid id);
    }
}
