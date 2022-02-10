using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Api.Interfaces;
using Exadel.OfficeBooking.Api.Services;
using Exadel.OfficeBooking.Domain.OfficePlan;
using Exadel.OfficeBooking.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<List<OfficeDto>>> GetOffices([FromQuery]OfficeFilterDto filterModel)
        {
            var offices =  await _officeService.GetOffices(filterModel);

            if (offices.Count == 0) return NoContent();

            return Ok(offices);
        }

        // GET api/<OfficesController>/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<OfficeDto>> GetOfficeById(Guid id)
        {
            var result = await _officeService.GetOfficeById(id);

            if (result == null) return NoContent();

            return Ok(result);
        }
            

       

        // POST api/<OfficesController>
        [HttpPost]
        [Produces("application/json")]
        public async Task<Guid?> Add([FromBody] OfficeDto office)
        {
            return await _officeService.SaveOffice(office);
            
        }

        //PUT api/<OfficesController>/5
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] OfficeDto office)
        {
            var officeUpdated = await _officeService.UpdateOffice(office);

            if (officeUpdated == null) return BadRequest();

            return Ok("Updated");
        }

       // DELETE api/<OfficesController>/5


        [HttpDelete("{id}")]
        public  async Task<ActionResult> Delete(Guid id)
        {
            var result = await _officeService.DeleteOffice(id);

            if (result == null) return NoContent();

            return Ok(result);
        }
    }
}
