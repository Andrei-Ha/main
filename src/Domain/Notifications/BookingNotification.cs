using System;
using Linq;
using Exadel.OfficeBooking.Domain;
using System.ComponentModel.DataAnnotations;


namespace Exadel.OfficeBooking.Domain.Notifications

{
    public class BookingNotification // : IBooking
    {
        [Required]
        [Display(Name = "To (Email Address)")]
        public string ToEmail { get; set; }
        //public int TelegramId { get; set; }
        public string SubjectOfMessage { get; set; }
        //public List<Booking> MessageBody { get; set; }
        public DateOnly SentDate { get; set; }
    }
}