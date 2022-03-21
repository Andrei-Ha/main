using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Api.Interfaces
{
    public interface IEmailService
    {
        void SendEmailTo(string receiverr, string body);
    }
}
