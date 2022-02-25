﻿using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        public override FsmState Execute(Update update, FsmState fsmState)
        {
            fsmState.Result = new Result
            {
                TextMessage = "What do you want to do today?",
                NextStep = nameof(ActionChoise),
                Propositions = new string[] { "Change or Cancel a booking", "Book a workplace", "Nothing" }
            };

            return fsmState;
        }
    }
}
