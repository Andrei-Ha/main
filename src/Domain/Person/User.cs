namespace Exadel.OfficeBooking.Domain.Person
{
    public class User : BaseModel
    {
        public int TelegramId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role{ get; set; }
        // The use of this property needs to be clarified!
        // public int Position { get; set; }
        public DateOnly EmploymentStart { get; set; }
        public DateOnly? EmploymentEnd { get; set; }
        public List<Vacation> Vacations { get; set; } = new();
        // public IEnumerable<Booking> BookingList { get; set; }
        // TBD Seat ID. User can set prefferred seat
        public string PrefferedSeat { get; set; } = string.Empty;
    }
}
