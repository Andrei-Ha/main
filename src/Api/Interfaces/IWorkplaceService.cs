using Exadel.OfficeBooking.Api.DTO;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IWorkplaceService
    {
        Task<List<WorkplaceDto>> GetWorkplaces(WorkplaceFilterDto filterModel);

        Task<WorkplaceDto?> GetWorkplaceById(Guid id);

        Task<WorkplaceDto?> CreateWorkplace(WorkplaceDto workplace);

        Task<WorkplaceDto?> UpdateWorkplace(WorkplaceDto workplace);

        Task<Guid?> DeleteWorkplaceById(Guid id);
    }
}
