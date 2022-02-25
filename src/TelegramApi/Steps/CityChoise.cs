using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class CityChoise : StateMachineStep
    {
        public CityChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override FsmState Execute(Update update, FsmState fsmState)
        {

            fsmState.Result = new Result();
            return fsmState;
        }
    }
}
