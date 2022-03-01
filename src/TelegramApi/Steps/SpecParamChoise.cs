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
    public class SpecParamChoise : StateMachineStep
    {
        public SpecParamChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (text == _fsmState.Result.Propositions[0])
            {
                _fsmState.IsSpecifyWorkplace = true;
                _fsmState.Result = new();
                return _fsmState;
            }
            else if (text == _fsmState.Result.Propositions[1])
            {
                _fsmState.IsSpecifyWorkplace = false;
            }
            else
            {
                return _fsmState;
            }
            _fsmState.Result.TextMessage = _fsmState.Summary() + "\n\nConfirm the booking?";
            _fsmState.Result.NextStep = nameof(Template);
            _fsmState.Result.Propositions = new[] { "confirm", "cancel" };
            return _fsmState;
        }
    }
}