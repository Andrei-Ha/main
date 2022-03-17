using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class AddBookingDto
{
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }

    public DateTime Date { get; set; }

    public BookingTypeEnum BookingType { get; set; }
    public string Summary { get; set; } = string.Empty;
}
