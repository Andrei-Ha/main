using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<WorkplaceDto>>> GetAll([FromQuery] WorkplaceFilterDto filterModel)
        {
            var workplaces = await _workplaceService.GetWorkplaces(filterModel);

            if (workplaces.Count == 0)
                return NoContent();

            return Ok(workplaces);
        }

        // GET api/<WorkplaceController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceDto>> GetById(Guid id)
        {
            var workplace = await _workplaceService.GetWorkplaceById(id);

            if (workplace == null)
                return NoContent();

            return Ok(workplace);
        }

        // POST api/<WorkplaceController>
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<WorkplaceDto>> Create([FromBody] WorkplaceDto workplace)
        {
            var workplaceCreated = await _workplaceService.CreateWorkplace(workplace);

            if (workplaceCreated == null)
                return BadRequest("Input model is null");

            var uri = new Uri($"{Request.Path.Value}/{workplaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, workplaceCreated.Id);
        }

        // PUT api/<WorkplaceController>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] WorkplaceDto workplace)
        {
            var workplaceUpdated = await _workplaceService.UpdateWorkplace(workplace);

            if (workplaceUpdated == null)
                return BadRequest("Input model is null");

            return Ok(workplaceUpdated);
        }

        // DELETE api/<WorkplaceController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var guid = await _workplaceService.DeleteWorkplaceById(id);

            if (guid == null)
                return NoContent();

            return Ok(id);
        }
    }
}
