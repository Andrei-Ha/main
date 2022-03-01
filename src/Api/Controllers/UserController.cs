using Exadel.OfficeBooking.Api.DTO.PersonDto;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Exadel.OfficeBooking.Api.DTO;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<GetUserDto[]> Get()
        {
            return await _db.Users
                .AsNoTracking()
                .Select(o => o.Adapt<GetUserDto>())
                .ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            User? user = await _db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound(new {message = "The requested user was not found"});
            }

            return Ok(user.Adapt<GetUserDto>());
        }
        
        //Gets user by his/her telegram id
        [HttpGet("telegram/{id}")]
        public async Task<IActionResult> GetByTelegramId(long id)
        {
            ServiceResponse<GetUserDto> response = new ServiceResponse<GetUserDto>();
            User? user = await _db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.TelegramId == id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User is not created";
                return BadRequest(response);
            }

            response.Data = user.Adapt<GetUserDto>();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Post(SetUserDto postUserDto )
        {
            User user = postUserDto.Adapt<User>();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Created($"User/{user.Id}", user.Adapt<GetUserDto>());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Put(Guid id, [FromBody] SetUserDto putUserDto)
        {
            User? user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "The user was not found" });
            }

            var userUpd = putUserDto.Adapt<User>();
            userUpd.Id = id;
            _db.Users.Update(userUpd);
            await _db.SaveChangesAsync();
            return Ok(userUpd.Adapt<GetUserDto>());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            User? user = await _db.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return NotFound(new { message = "The user doesn't exist" });
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Ok(user.Adapt<GetUserDto>());
        }
    }
}
