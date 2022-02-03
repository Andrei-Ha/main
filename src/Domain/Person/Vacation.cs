using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class Vacation : BaseModel
    {
        public User User { get; set; } = new User();
        public DateOnly VacationStart { get; set; }
        public DateOnly VacationEnd { get; set; }
    }
}
