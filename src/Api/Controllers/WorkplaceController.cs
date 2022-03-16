using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class WorkplaceController : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService;
        public WorkplaceController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        [Route("api/[controller]")]
        [HttpGet]
        public async Task<WorkplaceGetDto[]> GetFiltered([FromQuery] WorkplaceFilterDto filterModel)
        {
            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            return workplaces;
        }

        [Route("api/Office/{officeId?}/[controller]")]
        [HttpGet]
        public async Task<WorkplaceGetDto[]> GetFilteredInExactOffice ([FromRoute] Guid officeId, [FromQuery] WorkplaceFilterDto filterModel)
        {
            filterModel.OfficeId = officeId;

            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            return workplaces;
        }
        
        [Route("api/[controller]/{id}")]
        [HttpGet]
        public async Task<ActionResult<WorkplaceGetDto>> GetById(Guid id)
        {
            var workplace = await _workplaceService.GetWorkplaceById(id);

            if (workplace == null)
                return NotFound(new { message = "Requested workplace not found" });

            return Ok(workplace);
        }
        
        [Route("api/Map/{mapId}/[controller]")]
        [HttpPost]
        [Authorize(Roles ="Admin, MapEditor")]
        public async Task<ActionResult<WorkplaceGetDto>> Create([FromRoute] Guid mapId, WorkplaceSetDto workplace)
        {
            workplace.MapId = mapId;
            Console.WriteLine("MapId = " + workplace.MapId);
            var workplaceCreated = await _workplaceService.CreateWorkplace(workplace);

            var uri = new Uri($"api/Workplace/{workplaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, workplaceCreated);
        }
        
        [Route("api/[controller]/{id}")]
        [HttpPut]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<ActionResult<WorkplaceGetDto>> Update(Guid id, [FromBody] WorkplaceSetDto workplace)
        {
            var workplaceUpdated = await _workplaceService.UpdateWorkplace(id, workplace);

            if (workplaceUpdated == null)
                return NotFound(new { message = "Requested workplace not found" });

            return Ok(workplaceUpdated);
        }

        [Route("api/[controller]/{id}")]
        [HttpDelete]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workplaceDeleted = await _workplaceService.DeleteWorkplaceById(id);

            if (workplaceDeleted == null)
                return NotFound(new { message = "Requested workplace not found" });

            return NoContent();
        }
    }
}
