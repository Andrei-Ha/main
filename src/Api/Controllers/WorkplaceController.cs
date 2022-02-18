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
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class WorkplaceController : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService;
        public WorkplaceController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        // GET: api/workplace/getall
        [HttpGet]
        [Produces("application/json")]
        public async Task<WorkplaceGetDto[]> GetAll()
        {
            var workplaces = await _workplaceService.GetWorkplaces();

            return workplaces;
        }

        // GET: api/workplace/getfiltered
        [HttpGet]
        [Produces("application/json")]
        public async Task<WorkplaceGetDto[]> GetFiltered ([FromQuery] WorkplaceFilterDto filterModel)
        {
            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            return workplaces;
        }

        // GET api/workplace/getbyid/{guid}
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceGetDto>> GetById(Guid id)
        {
            var workplace = await _workplaceService.GetWorkplaceById(id);

            if (workplace == null)
                return NotFound(new { message = "Requested workplace not found" });

            return Ok(workplace);
        }

        // POST api/workplace/create
        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles ="Admin, MapEditor")]
        public async Task<ActionResult<WorkplaceGetDto>> Create([FromBody] WorkplaceSetDto workplace)
        {
            var workplaceCreated = await _workplaceService.CreateWorkplace(workplace);

            var uri = new Uri($"{Request.Path.Value}/{workplaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, workplaceCreated);
        }

        // PUT api/workplace/update
        [HttpPut]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<ActionResult<WorkplaceGetDto>> Update(Guid id, [FromBody] WorkplaceSetDto workplace)
        {
            var workplaceUpdated = await _workplaceService.UpdateWorkplace(id, workplace);

            if (workplaceUpdated == null)
                return NotFound(new { message = "Requested workplace not found" });

            return Ok(workplaceUpdated);
        }

        // DELETE api/workplace/delete/{guid}
        [HttpDelete("{id}")]
        [Produces("application/json")]
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
