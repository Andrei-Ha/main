using Exadel.OfficeBooking.Api.DTO.OfficeDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    [AllowAnonymous]
    public class ParkingPlaceController : ControllerBase
    {
        private readonly IParkingPlaceService _parkingPlaceService;
        public ParkingPlaceController(IParkingPlaceService parkingPlaceService)
        {
            _parkingPlaceService = parkingPlaceService;
        }

        [HttpGet]
        public async Task<ParkingPlaceGetDto[]> GetParkings()
        {
            var parkings = await _parkingPlaceService.GetParkings();

            return parkings;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingPlaceGetDto>> GetById(Guid id)
        {
            var parking = await _parkingPlaceService.GetParkingById(id);

            if (parking == null)
                return NotFound(new { message = "The requested parking was not found" });

            return Ok(parking);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<ActionResult<ParkingPlaceGetDto>> Create([FromBody] ParkingPlaceSetDto parking)
        {
            var ParkingPlaceCreated = await _parkingPlaceService.CreateParking(parking);

            var uri = new Uri($"{Request.Path.Value}/{ParkingPlaceCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, ParkingPlaceCreated);
        }

        [HttpPut("put/{id}")]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<ActionResult<ParkingPlaceGetDto>> Update(Guid id, [FromBody] ParkingPlaceSetDto parking)
        {
            var parkingPlaceUpdated = await _parkingPlaceService.UpdateParking(id, parking);

            if (parkingPlaceUpdated == null)
                return NotFound(new { message = "Requested parking not found" });

            return Ok(parkingPlaceUpdated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, MapEditor")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var parkingPlaceDeleted = await _parkingPlaceService.DeleteParking(id);

            if (parkingPlaceDeleted == null)
                return NotFound(new { message = "Requested parking not found" });

            return NoContent();
        }
    }
}
