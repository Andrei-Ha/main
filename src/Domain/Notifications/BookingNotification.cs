using System;

namespace Exadel.OfficeBooking.Domain.Notifications
{
    public class BookingNotification
    {
        public string EmailAdress { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string MessageBody { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }
    }
}
