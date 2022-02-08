using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        AppDbContext db;
        public UsersController(AppDbContext context)
        {
            db = context;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.Users.ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            User? user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Results.NotFound(new {message = "The requested user was not found"});
            }

            return Results.Json(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IResult> Post(User user)
        {
            if (user == null)
            {
                return Results.BadRequest();
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"Users/{user.Id}", user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] User userData)
        {
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return Results.NotFound(new { message = "The user was not found" });
            }

            if (userData.FirstName != string.Empty) { user.FirstName = userData.FirstName;}
            if (userData.LastName != string.Empty) { user.LastName = userData.LastName;}
            if (userData.Email != string.Empty) { user.Email = userData.Email;}

            await db.SaveChangesAsync();
            return Results.Json(user);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return Results.BadRequest(new { message = "Guid is Empty" });
            }

            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return Results.NotFound(new { message = "The user not exist" });
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.Json(user);
        }
    }
}
