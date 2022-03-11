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
    public class ConfirmBooking : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null || _state.Propositions.Count < 0)
            {
                return _state;
            }

            // Confirm
            if (text == _state.Propositions[0])
            {
                _state.TextMessage = "A new booking has been created.\nAll details have been sent to you by email.\nBye!";
            }
            // Cancel
            else if (text == _state.Propositions[1])
            {
                _state.TextMessage = "You have canceled your booking.\nBye!";
            }

            _state.Propositions = new();
            _state.NextStep = "Finish";
            return _state;
        }
    }
}