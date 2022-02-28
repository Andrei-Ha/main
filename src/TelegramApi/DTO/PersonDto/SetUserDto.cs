using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.PersonDto
{
    public class SetUserDto
    {
        public int TelegramId { get; set; }
        public string StepName { get; set; } = "Greeting";

        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        
        public DateTime? EmploymentStart { get; set; }
        public DateTime? EmploymentEnd { get; set;}

        public UserRole Role { get; set; } = UserRole.CommonUser;
    }
}
