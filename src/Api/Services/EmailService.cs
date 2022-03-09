using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Services
{
    public class EmailService
    {
      
        public static void SendEmailTo(string receiver, string body)
        {

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

