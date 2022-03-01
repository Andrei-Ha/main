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
    public class ActionChoise : StateMachineStep
    {
        public ActionChoise(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            // Change or Cancel a booking
            if (text == _fsmState.Result.Propositions[0]) 
            {
                _fsmState.Result = new Result
                {

                };
            }

            // Book a workplace
            else if (text == _fsmState.Result.Propositions[1])
            {
                string[] prepositions = Array.Empty<string>();
                var httpResponse = await GetModelFromWebAPI<IEnumerable<OfficeGetDto>>("office");
                IEnumerable<OfficeGetDto>? offices = httpResponse?.Model;
                if (offices != null)
                {
                    _fsmState.Result = new Result
                    {
                        TextMessage = "Select a city:",
                        NextStep = nameof(CityChoise),
                        Propositions = offices.Select(o => o.City).OrderBy(p => p).Distinct().ToArray(),
                    };
                }
                else
                {
                    _fsmState.Result.TextMessage = $"\nStatusCode: {httpResponse?.StatusCode.ToString()}";
                }
            }

            // Nothing
            else if (text == _fsmState.Result.Propositions[2])
            {
                _fsmState.Result = new Result
                {
                    TextMessage = "Bye! See you later",
                    //NextStep = "Finish"
                };
            }

            return _fsmState;
        }
    }
}
