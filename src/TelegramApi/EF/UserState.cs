using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using System;

namespace Exadel.OfficeBooking.TelegramApi.EF
{
    public class UserState
    {
        public int Id { get; set; }

        public long ChatId { get; set; }

        public string StepName { get; set; } = nameof(Greetings);

        public string TextMessage { get; set; } = "Not implemented yet";

        public Guid? UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public Guid? OfficeId { get; set; }

        public Guid? MapId { get; set; }

        public Guid? Workplace { get; set; }

        public DateTime? BookDate { get; set; }

        public bool IsSuccess { get; set; } = false;
    }
}
