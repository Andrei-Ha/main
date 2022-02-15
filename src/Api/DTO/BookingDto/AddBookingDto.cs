using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class AddBookingDto
{
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }

    public DateTime StartDate { get; set; }
}
