using Exadel.OfficeBooking.Api.DTO.WorkplaceDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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
        public async Task<ActionResult<WorkplaceGetDto[]>> GetAll()
        {
            var workplaces = await _workplaceService.GetWorkplaces();

            return Ok(workplaces);
        }

        // GET: api/workplace/getfiltered
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceGetDto[]>> GetFiltered ([FromQuery] WorkplaceFilterDto filterModel)
        {
            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            return Ok(workplaces);
        }

        // GET api/workplace/getbyid/{guid}
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var workplace = await _workplaceService.GetWorkplaceById(id);

            if (workplace == null)
                return NoContent();

            return Ok(workplace);
        }

        // POST api/workplace/create
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] WorkplaceSetDto workplace)
        {
            var workplaceCreated = await _workplaceService.CreateWorkplace(workplace);

            if (workplaceCreated == null)
                return BadRequest("Input model is null");

            var uri = new Uri($"{Request.Path.Value}/{workplaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, workplaceCreated.Id);
        }

        // PUT api/workplace/update
        [HttpPut]
        [Produces("application/json")]
        public async Task<IActionResult> Update([FromBody] WorkplaceGetDto workplace)
        {
            var workplaceUpdated = await _workplaceService.UpdateWorkplace(workplace);

            if (workplaceUpdated == null)
                return BadRequest("Input model is null");

            return Ok(workplaceUpdated);
        }

        // DELETE api/workplace/delete/{guid}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _workplaceService.DeleteWorkplaceById(id);

            if (result == null)
                return BadRequest("Input model is null");

            return Ok(result);
        }
    }
}
