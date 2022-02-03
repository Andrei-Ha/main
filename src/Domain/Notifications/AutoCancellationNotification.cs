using System;
using Linq;
using Exadel.OfficeBooking.Domain;
using Exadel.OfficeBooking.Domain.Person;
using System.ComponentModel.DataAnnotations;


namespace Exadel.OfficeBooking.Domain.Notifications

{
    public class AutoCancellationNotification // : IBooking
    {
        [Required]
        [Display(Name = "To (Email Address)")]
        public string ToEmail { get; set; }
        //public int TelegramId { get; set; }
        public string SubjectOfMessage { get; set; }
        public DateOnly SentDate { get; set; }
    }
}