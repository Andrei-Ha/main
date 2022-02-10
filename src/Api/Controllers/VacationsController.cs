using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/vacation")]
    [ApiController]
    public class VacationsController : ControllerBase
    {
        private AppDbContext _db;
        public VacationsController(AppDbContext appDbContext)
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
        public async Task<IResult> Get(Guid id)
        {
            Vacation? vacation = await _db.Vacations
                .AsNoTracking()
                .Include(o => o.User)
                .SingleOrDefaultAsync(v => v.Id == id);
            if (vacation == null)
            {
                return Results.NotFound(new { message = "Requested vacation doesn't exist" });
            }
                
            return Results.Ok(vacation.Adapt<GetVacationDto>());
        }

        [HttpPost]
        public async Task<IResult> Post(PostVacationDto postVacationDto)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(x => x.Id == postVacationDto.UserId);
            if (user == null)
            {
                return Results.NotFound(new { message = "User to add vacation not found" });
            }
            
            if (user.Vacations == null)
            {
                user.Vacations = new List<Vacation>();
            }

            Vacation vacation = postVacationDto.Adapt<Vacation>();
            user.Vacations.Add(vacation);
            await _db.SaveChangesAsync();
            return Results.Created($"vacation/{vacation.Id}", vacation.Adapt<GetVacationDto>());
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, PutVacationDto putVacationDto)
        {
            Vacation? vacation = await _db.Vacations.SingleOrDefaultAsync(v => v.Id == id);
            if (vacation == null)
            {
                return Results.NotFound(new { message = "Requested vacation doesn’t exist" });
            }
            
            User? user = await _db.Users.FirstOrDefaultAsync(x => x.Id==putVacationDto.UserId);
            if (user == null)
            {
                return Results.NotFound(new { message = "The user was not found" });
            }

            vacation.User = user;
            vacation.VacationStart = putVacationDto.VacationStart;
            vacation.VacationEnd = putVacationDto.VacationEnd;
            await _db.SaveChangesAsync();
            return Results.Ok(vacation.Adapt<GetVacationDto>());
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id)
        { 
            Vacation? vacation = _db.Vacations.FirstOrDefault(x => x.Id == id);
            if (vacation == null)
            {
                return Results.NotFound(new { message = "Requested vacation doesn’t exist" });
            }

            _db.Vacations.Remove(vacation);
            await _db.SaveChangesAsync();
            return Results.Ok();
        }
    }
}
