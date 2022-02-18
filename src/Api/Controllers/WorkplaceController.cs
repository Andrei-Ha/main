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

        // GET: api/<WorkplaceController>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceGetDto[]>> GetAll([FromQuery] WorkplaceFilterDto filterModel)
        {
            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            return Ok(workplaces);
        }

        // GET api/<WorkplaceController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceSetDto>> GetById(Guid id)
        {
            var workplace = await _workplaceService.GetWorkplaceById(id);

            if (workplace == null)
                return NoContent();

            return Ok(workplace);
        }

        // POST api/<WorkplaceController>
        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles ="Admin, MapEditor")]
        public async Task<IActionResult> Create([FromBody] WorkplaceSetDto workplace)
        {
            var workplaceCreated = await _workplaceService.CreateWorkplace(workplace);

            if (workplaceCreated == null)
                return BadRequest("Input model is null");

            var uri = new Uri($"{Request.Path.Value}/{workplaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, workplaceCreated.Id);
        }

        // PUT api/<WorkplaceController>
        [HttpPut]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<IActionResult> Update([FromBody] WorkplaceGetDto workplace)
        {
            var workplaceUpdated = await _workplaceService.UpdateWorkplace(workplace);

            if (workplaceUpdated == null)
                return BadRequest("Input model is null");

            return Ok(workplaceUpdated);
        }

        // DELETE api/<WorkplaceController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, MapEdiros")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _workplaceService.DeleteWorkplaceById(id);

            if (result == null)
                return NoContent();

            return Ok(result);
        }
    }
}
