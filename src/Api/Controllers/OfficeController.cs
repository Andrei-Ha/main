using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;
        public OfficeController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpGet]
        public async Task<OfficeGetDto[]> GetOffices()
        {
            var offices = await _officeService.GetOffices();
            return offices;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeGetDto>> GetById(Guid id)
        {
            var result = await _officeService.GetOfficeById(id);

            if (result == null) return NotFound(new {message = "The requested office was not found"});

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OfficeSetDto office)
        {
            var officeCreated = await _officeService.CreateOffice(office);
            var uri = new Uri($"{Request.Path.Value}/{officeCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, officeCreated);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] OfficeGetDto office)
        {
            var officeUpdated = await _officeService.UpdateOffice(id, office);

            if (officeUpdated == null) return NotFound(new {message = "The office was not found"});

            return Ok(officeUpdated);
        }

        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(Guid id)
        {
            var result = await _officeService.DeleteOffice(id);

            if (result == null) return NotFound(new {message = "Requested office doesn't exist"});

            return Ok(result);
        }
    }
}
