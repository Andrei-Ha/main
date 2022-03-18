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
    public class temp____ : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // yes, I want to make one
            if (text == _state.Propositions[0])
            {

            }
            // no, thanks
            else if (text == _state.Propositions[1])
            {
                _state.SetByeAndFinish();
            }
 
            return _state;
        }
    }
}