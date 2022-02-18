using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class UpdateBookingDto
{
    public Guid Id { get; set; }
    public Guid WorkplaceId { get; set; }
    public DateTime Date { get; set; }
}
