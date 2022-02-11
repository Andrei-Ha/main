using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IWorkplaceService
    {
        Task<WorkplaceGetDto[]> GetWorkplaces(WorkplaceFilterDto filterModel);

        Task<WorkplaceGetDto?> GetWorkplaceById(Guid id);

        Task<WorkplaceGetDto?> CreateWorkplace(WorkplaceSetDto workplace);

        Task<WorkplaceGetDto?> UpdateWorkplace(WorkplaceGetDto workplace);

        Task<Guid?> DeleteWorkplaceById(Guid id);
    }
}
