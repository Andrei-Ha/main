using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class Result
    {
        public string TextMessage { get; set; } = "Not implemented yet";

        public List<string>? Propositions { get; set; } = default;

        public bool IsSendMessage = true;
    }
}
