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
    public class ActionChoise : StateMachineStep
    {
        public ActionChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override FsmState Execute(Update update, FsmState fsmState)
        {
            string? text = update.Message?.Text;
            // Change or Cancel a booking
            if (text == fsmState.Result.Propositions[0]) 
            {
                fsmState.Result = new Result
                {

                };
            }

            // Book a workplace
            else if (text == fsmState.Result.Propositions[1])
            {
                string[] prepositions = Array.Empty<string>();
                IEnumerable<OfficeGetDto>? offices = JsonConvert.DeserializeObject<IEnumerable<OfficeGetDto>>(GetJsonFromWebAPI("office").Result);
                if (offices != null) 
                { 
                    prepositions = offices.Select(o => o.City).OrderBy(p => p).Distinct().ToArray();
                }
                fsmState.Result = new Result
                {
                    TextMessage = "Select a city:",
                    NextStep = nameof(CityChoise),
                    Propositions = prepositions,
                };
            }

            // Nothing
            else if (text == fsmState.Result.Propositions[2])
            {
                fsmState.Result = new Result
                {
                    TextMessage = "Bye! See you later",
                    //NextStep = "Finish"
                };
            }

            return fsmState;
        }
    }
}
