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
    public class ParkingChoise : StateMachineStep
    {
        public ParkingChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (text == _fsmState.Result.Propositions[0])
            {
                _fsmState.IsParkingPlace = true;
            }
            else if (text == _fsmState.Result.Propositions[1])
            {
                _fsmState.IsParkingPlace = false;
            }
            else
            {
                return _fsmState;
            }
            _fsmState.Result.TextMessage = "Would you like to specify workplace parameners?";
            _fsmState.Result.NextStep = nameof(SpecParamChoise);
            _fsmState.Result.Propositions = new() { "Yes, I have special preferences", "No, I can take any available workplace" };
            return _fsmState;
        }
    }
}