using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO.MapDto;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;
        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        // GET
        [HttpGet]
        public async Task<MapGetDto[]> GetMap([FromQuery] MapFilterDto mapFilterDto)
        {
            var maps = await _mapService.GetMaps(mapFilterDto);

            return maps;
        }
        //

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MapGetDto>> GetById(Guid id)
        {
            var map = await _mapService.GetMapById(id);

            if (map == null)
                return NotFound(new { message = "The requested map (floor) not found" });

            return Ok(map);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<MapGetDto>> Create([FromBody] MapSetDto map)
        {
            var mapCreated = await _mapService.CreateMap(map);

            var uri = new Uri($"{Request.Path.Value}/{mapCreated.Id}".ToLower(), UriKind.Relative);

            return Created(uri, mapCreated);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult<MapGetDto>> Update(Guid id, [FromBody] MapSetDto map)
        {
            var mapUpdated = await _mapService.UpdateMap(id, map);

            if (mapUpdated == null)
                return NotFound(new { message = "Requested map (floor) not found" });

            return Ok(mapUpdated);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var mapDeleted = await _mapService.DeleteMap(id);

            if (mapDeleted == null)
                return NotFound(new { message = "Requested map (floor) not found" });

            return NoContent();
        }
    }
}
