using System;
using System.Collections.Generic;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public class Result
    {
        public Guid Id { get; set; }

        public string TextMessage { get; set; } = "Not implemented yet";

        public string NextStep { get; set; } = "Finish";

        public List<string>? Propositions { get; set; } = new();

        public Guid FsmStateId { get; set; }

        public FsmState? FsmState { get; set; }
    }
}
