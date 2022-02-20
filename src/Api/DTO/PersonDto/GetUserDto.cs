using System;

namespace Exadel.OfficeBooking.Api.DTO.PersonDto
{
    public class GetUserDto : SetUserDto
    {
        public Guid Id { get; set; }
    }
}
