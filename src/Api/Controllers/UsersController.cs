﻿using Exadel.OfficeBooking.Api.DTO;
using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private AppDbContext _db;
        public UsersController(AppDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<GetUserDto[]> Get()
        {
            return await _db.Users.AsNoTracking().Select(o => o.Adapt<GetUserDto>()).ToArrayAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResult> Get(Guid id)
        {
            User? user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return Results.NotFound(new {message = "The requested user was not found"});
            }

            return Results.Ok(user.Adapt<GetUserDto>());
        }

        [HttpPost]
        public async Task<IResult> Post(PostUserDto postUserDto )
        {
            User user = postUserDto.Adapt<User>();
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Results.Created($"Users/{user.Id}", user.Adapt<GetUserDto>());
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(Guid id, [FromBody] PutUserDto putUserDto)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return Results.NotFound(new { message = "The user was not found" });
            }

            user.TelegramId = putUserDto.TelegramId;
            user.FirstName = putUserDto.FirstName;
            user.LastName = putUserDto.LastName;
            user.Email = putUserDto.Email;
            user.EmploymentStart = putUserDto.EmploymentStart;
            user.EmploymentEnd = putUserDto.EmploymentEnd;
            await _db.SaveChangesAsync();
            return Results.Json(user.Adapt<GetUserDto>());
        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(Guid id)
        {
            User? user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return Results.NotFound(new { message = "The user doesn't exist" });
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return Results.Json(user.Adapt<GetUserDto>());
        }
    }
}
