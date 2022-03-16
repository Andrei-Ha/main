using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;

public class AddBookingDto
{
    public Guid UserId { get; set; }
    public Guid WorkplaceId { get; set; }

    public DateTime Date { get; set; }
}
