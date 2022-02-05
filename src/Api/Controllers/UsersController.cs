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
            /*if (!db.Users.Any())
            {
                db.Users.Add(new User { Name = "Tom", Age = 26 });
                db.Users.Add(new User { Name = "Alice", Age = 31 });
                db.SaveChanges();
            }*/
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.Users.ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
