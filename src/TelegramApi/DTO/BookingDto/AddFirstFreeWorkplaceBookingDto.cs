using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO.BookingDto
{
    public class AddFirstFreeWorkplaceBookingDto
    {
        public Guid UserId { get; set; }
        public Guid OfficeId { get; set; }

        public DateTime Date { get; set; }
    }
}
