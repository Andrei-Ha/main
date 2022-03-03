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
        public ParkingChoice(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_fsmState.Propositions == null)
            {
                return _fsmState;
            }

            // yes
            if (text == _fsmState.Propositions[0])
            {
                _fsmState.IsParkingPlace = true;
            }
            // no
            else if (text == _fsmState.Propositions[1])
            {
                _fsmState.IsParkingPlace = false;
            }
            else
            {
                return _fsmState;
            }

            _fsmState.TextMessage = "Would you like to specify workplace parameners?";
            _fsmState.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
            _fsmState.NextStep = nameof(SpecParamChoice);
            return _fsmState;
        }
    }
}