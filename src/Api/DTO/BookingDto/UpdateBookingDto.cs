using System;

namespace Exadel.OfficeBooking.Api.DTO.BookingDto;

public class UpdateBookingDto
{
    public Guid Id { get; set; }
    public Guid WorkplaceId { get; set; }
    public DateTime Date { get; set; }

    public BookingTypeEnum BookingType { get; set; }
    public string Summary { get; set; } = string.Empty;
}
