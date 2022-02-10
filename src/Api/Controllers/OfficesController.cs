using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OfficesController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        public OfficesController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        // GET: api/<OfficesController>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<OfficeGetDto[]>> GetOffices([FromQuery]OfficeFilterDto filterModel)
        {
            var offices = await _officeService.GetOffices(filterModel);

            return Ok(offices);
        }

        // GET api/<OfficesController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<OfficeGetDto>> GetById(Guid id)
        {
            var result = await _officeService.GetOfficeById(id);

            if (result == null) return NoContent();

            return Ok(result);
        }

        // POST api/<OfficesController>
        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] OfficeSetDto office)
        {
            var officeCreated = await _officeService.CreateOffice(office);

            if (officeCreated == null)
                return BadRequest("Input model is null");

            var uri = new Uri($"{Request.Path.Value}/{officeCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, officeCreated.Id);
        }

        //PUT api/<OfficesController>/5
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] OfficeGetDto office)
        {
            var officeUpdated = await _officeService.UpdateOffice(office);

            if (officeUpdated == null) return BadRequest();

            return Ok("Updated");
        }

       // DELETE api/<OfficesController>/5


        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(Guid id)
        {
            var result = await _officeService.DeleteOffice(id);

            if (result == null) return NoContent();

            return Ok(result);
        }
    }
}
