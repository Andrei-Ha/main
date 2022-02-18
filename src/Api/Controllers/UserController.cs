using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private AppDbContext _db;
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

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Post(PostUserDto postUserDto )
        {
            User user = postUserDto.Adapt<User>();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Created($"User/{user.Id}", user.Adapt<GetUserDto>());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PutUserDto putUserDto)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "The user was not found" });
            }

            user.ChatId = putUserDto.ChatId;
            user.FirstName = putUserDto.FirstName;
            user.LastName = putUserDto.LastName;
            user.Email = putUserDto.Email;
            user.EmploymentStart = putUserDto.EmploymentStart;
            user.EmploymentEnd = putUserDto.EmploymentEnd;
            await _db.SaveChangesAsync();
            return Ok(user.Adapt<GetUserDto>());
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
