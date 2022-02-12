namespace Exadel.OfficeBooking.TelegramApi
{
    public class TelegramModel
    {
        public Guid ChatId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Username { get; set; }

        public string? Message { get; set; }
    }
}
