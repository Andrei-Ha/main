using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.DTO
{
    public class BookView
    {
        public Guid Id { get; set; }

        public int MessageId { get; set; }

        public string BookingId { get; set; } = string.Empty;

        public bool IsChecked { get; set; } = false;

        public Guid UserStateId { get; set; }
    }

    public class BookViewResponse
    {
        public List<BookView> BookViews { get; set; } = new();

        public int BackMessageId = 0;

        public bool IsAllChecked = false;
    }
}
