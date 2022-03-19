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
    public class EditingChoise : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // I want to change office
            if (text == _state.Propositions[0])
            {
                _state.SetByeAndFinish();
            }
            // I want to change workplace in the same office
            else if (text == _state.Propositions[1])
            {
                _state.SetByeAndFinish();
            }
            // I want to change my booking dates
            else if (text == _state.Propositions[2])
            {
                _state.SetByeAndFinish();
            }
 
            return _state;
        }
    }
}