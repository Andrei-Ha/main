using Exadel.OfficeBooking.Api.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private bool _emailOn;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  void SendEmailTo(string receiver, string body)
        {
            _emailOn = bool.Parse(_configuration["EmailOptions:SendEmail"]);
            if (!_emailOn) return;

            MailMessage mail = new MailMessage("Team3InfoBot@gmail.com", receiver);
            mail.Subject = "Booking info";
            mail.Body = "Your Booking was succesfull" + "\n" + body;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("Team3InfoBot@gmail.com", "SandBox7733");

            try
            {
                client.Send(mail);
            }
            catch (Exception)
            {
                throw new SmtpFailedRecipientsException();
            }

        }
    }
}

