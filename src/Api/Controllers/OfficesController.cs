using Exadel.OfficeBooking.Api.DTO.officeDto;
using Exadel.OfficeBooking.Api.DTO.OfficeDto;
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
        public async Task<List<CreateOfficeDto>> GetOffices([FromQuery]OfficeFilterDto filterModel)
        {
            return await _officeService.GetOffices(filterModel);
        }

        // GET api/<OfficesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateOfficeDto>> GetOfficeById(Guid id)
        {
            var result = await _officeService.GetOfficeById(id);

            if (result == null) return new NoContentResult();

            return result;
        }
            

       

        // POST api/<OfficesController>
        [HttpPost]
        public async Task<Guid> Add([FromBody] CreateOfficeDto office)
        {
            return await _officeService.SaveOffice(office);
            
        }

        // PUT api/<OfficesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<OfficesController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _officeService.DeleteOffice(id);
        }
    }
}
