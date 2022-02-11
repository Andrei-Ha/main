using System;
using System.ComponentModel.DataAnnotations;

namespace Exadel.OfficeBooking.Api.DTO
{
    public class VacationDto
    {
        [Required]
        public DateTime VacationStart { get; set; }
        
        [Required]
        public DateTime VacationEnd { get; set; }
    }

    public class PostVacationDto : VacationDto
    {
        public Guid UserId { get; set; }
    }

    public class GetVacationDto : PostVacationDto
    {
        public Guid Id { get; set; }
    }

    public class PutVacationDto : GetVacationDto
    {

    }
}
