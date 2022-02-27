using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.EF
{
    public class UserState
    {
        public int Id { get; set; }

        public long ChatId { get; set; }

        public string StepName { get; set; } = nameof(Greetings);

        public string TextMessage { get; set; } = "Not implemented yet";

        public Guid? UserId { get; set; }

        public List<Propositions>? Propositions { get; set; }
    }
}
