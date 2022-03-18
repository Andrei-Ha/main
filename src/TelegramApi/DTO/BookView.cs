using System;

namespace Exadel.OfficeBooking.TelegramApi.DTO
{
    public class BookView
    {
        public Guid Id { get; set; }

        public int MessageId { get; set; }

        public Guid BookingId { get; set; }

        public bool IsChecked { get; set; } = false;

        public Guid UserStateId { get; set; }
    }
}
