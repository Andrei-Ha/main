using Exadel.OfficeBooking.Domain.Person;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exadel.OfficeBooking.Api.DTO.PersonDto
{
    public class SetUserDto
    {
        public int TelegramId { get; set; }

        [Required]
        public string FirstName { get; set; } = String.Empty;
        
        [Required]
        public string LastName { get; set; } = String.Empty;

        [Required]
        public string Email { get; set; } = String.Empty;
        
        [Required]
        public DateTime? EmploymentStart { get; set; }
        
        public DateTime? EmploymentEnd { get; set;}

        public UserRole Role { get; set; } = UserRole.CommonUser;
    }
}
