using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.PersonDto
{
    public class LoginUserDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public UserRole Role { get; set; }

        public string Token { get; set; } = string.Empty;

        public Guid UserStateId { get; set; }
        
        public UserState? UserState { get; set; }
    }
}
