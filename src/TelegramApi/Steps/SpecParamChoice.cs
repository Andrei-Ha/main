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
        public SpecParamChoice(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_fsmState.Propositions == null)
            {
                return _fsmState;
            }

            // Yes, I have special preferences
            if (text == _fsmState.Propositions[0])
            {
                _fsmState.IsSpecifyWorkplace = true;
                _fsmState.SetResult();
            }
            // No, I can take any available workplace
            else if (text == _fsmState.Propositions[1])
            {
                _fsmState.IsSpecifyWorkplace = false;
                _fsmState.TextMessage = _fsmState.Summary() + "\n\nConfirm the booking?";
                _fsmState.Propositions = new() { "confirm", "cancel" };
                _fsmState.NextStep = nameof(Template);
            }
            return _fsmState;
        }
    }
}