using System;

namespace Exadel.OfficeBooking.Domain.Person
{
    public class Vacation : BaseModel
    {
        public User User { get; set; } = new();
        public DateTime VacationStart { get; set; }
        public DateTime VacationEnd { get; set; }
    }
}
