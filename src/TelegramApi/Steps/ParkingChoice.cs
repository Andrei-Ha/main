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
    public class ParkingChoice : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // yes
            if (text == _state.Propositions[0])
            {
                _state.IsParkingPlace = true;
            }
            // no
            else if (text == _state.Propositions[1])
            {
                _state.IsParkingPlace = false;
            }
            else
            {
                return _state;
            }

            _state.TextMessage = "Would you like to specify workplace parameners?";
            _state.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
            _state.NextStep = nameof(SpecParamChoice);
            return _state;
        }
    }
}