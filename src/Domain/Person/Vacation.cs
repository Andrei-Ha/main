using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly VacationStart { get; set; }
        public DateOnly VacationEnd { get; set; }
    }
}
