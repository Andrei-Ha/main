using System.ComponentModel.DataAnnotations;

namespace Exadel.OfficeBooking.Api.DTO
{
    public class UserDto
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
    }

    public class GetUserDto :UserDto
    {
        public Guid Id { get; set; }
     
        public string Role { get; set; } = string.Empty; 
    }

    public class PostUserDto : UserDto
    {

    }

    public class PutUserDto : UserDto
    {

    }
}
