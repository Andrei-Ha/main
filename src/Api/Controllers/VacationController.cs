using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class VacationController : ControllerBase
    {
        private AppDbContext _db;
        public VacationController(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }

        [HttpGet]
        public async Task<GetVacationDto[]> Get()
        {
            return await _db.Vacations
                .Include(o => o.User)
                .AsNoTracking()
                .Select(o => o.Adapt<GetVacationDto>())
                .ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Vacation? vacation = await _db.Vacations
                .Include(o => o.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.Id == id);
            if (vacation == null)
            {
                return NotFound(new { message = "Requested vacation doesn't exist" });
            }
                
            return Ok(vacation.Adapt<GetVacationDto>());
        }

        [HttpPost]
        public async Task<IActionResult> Post(PostVacationDto postVacationDto)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(x => x.Id == postVacationDto.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User to add vacation not found" });
            }
            
            if (user.Vacations == null)
            {
                user.Vacations = new List<Vacation>();
            }

            Vacation vacation = postVacationDto.Adapt<Vacation>();
            user.Vacations.Add(vacation);
            await _db.SaveChangesAsync();
            return Created($"vacation/{vacation.Id}", vacation.Adapt<GetVacationDto>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PutVacationDto putVacationDto)
        {
            Vacation? vacation = await _db.Vacations.SingleOrDefaultAsync(v => v.Id == id);
            if (vacation == null)
            {
                return NotFound(new { message = "Requested vacation doesn’t exist" });
            }
            
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Id==putVacationDto.UserId);
            if (user == null)
            {
                return NotFound(new { message = "The user was not found" });
            }

            vacation.User = user;
            vacation.VacationStart = putVacationDto.VacationStart;
            vacation.VacationEnd = putVacationDto.VacationEnd;
            await _db.SaveChangesAsync();
            return Ok(vacation.Adapt<GetVacationDto>());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles= "Admin")]
        
        public async Task<IActionResult> Delete(Guid id)
        { 
            Vacation? vacation = _db.Vacations.FirstOrDefault(x => x.Id == id);
            if (vacation == null)
            {
                return NotFound(new { message = "Requested vacation doesn’t exist" });
            }

            _db.Vacations.Remove(vacation);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
