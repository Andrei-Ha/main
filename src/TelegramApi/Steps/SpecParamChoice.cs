using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class SpecParamChoice : StateMachineStep
    {
        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // Yes, I have special preferences
            if (text == _state.Propositions[0])
            {
                _state.IsSpecifyWorkplace = true;
                _state.SetResult();
            }
            // No, I can take any available workplace
            else if (text == _state.Propositions[1])
            {
                _state.IsSpecifyWorkplace = false;
                _state.TextMessage = _state.Summary() + "\n\nConfirm the booking?";
                _state.Propositions = new() { "confirm", "cancel" };
                _state.NextStep = nameof(Template);
            }
            return _state;
        }
    }
}