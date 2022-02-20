using Exadel.OfficeBooking.Domain.Person;
using System;

namespace Exadel.OfficeBooking.Api.DTO.PersonDto
{
    public class LoginUserDto
    {
        public int TelegramId { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public UserRole Role { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
