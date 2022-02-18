using Exadel.OfficeBooking.Domain.Person;
using Exadel.OfficeBooking.EF;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Exadel.OfficeBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public LoginController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _db = appDbContext;
            _config = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] int TelegramId)
        {
            User? user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(t => t.TelegramId == TelegramId);
            if (user == null)
            {
                return Ok();
            }
            var loginUserDto = user.Adapt<LoginUserDto>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            
            // create JWT-token
            var jwt = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            loginUserDto.Token = token;
            return Ok(loginUserDto);
        }
    }

    public class LoginUserDto
    {
        public int TelegramId { get; set; }

        public string? UserName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
