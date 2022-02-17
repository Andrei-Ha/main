using System;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;
        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        // GET
        [HttpGet]
        public async Task<MapDto[]> GetMap()
        {
            var maps = await _mapService.GetMap();
            return maps;
        }
        //

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<MapDto>> GetById(Guid id)
        {
            var result = await _mapService.GetMapById(id);
            if (result == null) return NotFound(new { message = "The requested map (floor) not found" });
            return Ok(result);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MapDto map)
        {
            var mapCreated = await _mapService.CreateMap(map);
            var uri = new Uri($"{Request.Path.Value}/{mapCreated.Id}".ToLower(), UriKind.Relative);
            return Created(uri, mapCreated);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] MapDto map)
        {
            var mapUpdated = await _mapService.UpdateMap(id, map);
            if (mapUpdated == null) return NotFound(new { message = "Map (floor) not found" });
            return Ok(mapUpdated);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mapService.DeleteMap(id);
            if (result == null) return NotFound(new { message = "Requested map (floor) not found" });
            return Ok(result);
        }
    }
}
