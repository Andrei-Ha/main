using System;
using System.ComponentModel.DataAnnotations;

namespace Exadel.OfficeBooking.Api.DTO.PersonDto
{
    public class SetVacationDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime VacationStart { get; set; }
        
        [Required]
        public DateTime VacationEnd { get; set; }
    }
}
