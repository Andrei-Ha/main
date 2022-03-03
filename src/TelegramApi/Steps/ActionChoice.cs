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
    public class ActionChoice : StateMachineStep
    {
        public ActionChoice(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_fsmState.Propositions == null)
            {
                return _fsmState;
            }

            // Change or Cancel a booking
            if (text == _fsmState.Propositions[0]) 
            {
                _fsmState.SetResult();
            }

            // Book a workplace
            else if (text == _fsmState.Propositions[1])
            {
                var httpResponse = await GetModelFromWebAPI<IEnumerable<OfficeGetDto>>("office");
                IEnumerable<OfficeGetDto>? offices = httpResponse?.Model;
                if (offices != null)
                {
                    _fsmState.TextMessage = "Select a city:";
                    _fsmState.Propositions = offices.Select(o => o.City).OrderBy(p => p).Distinct().ToList();
                    _fsmState.NextStep = nameof(CityChoice);
                }
                else
                {
                    _fsmState.TextMessage = $"\nStatusCode: {httpResponse?.StatusCode.ToString()}";
                }
            }

            // Nothing
            else if (text == _fsmState.Propositions[2])
            {
                _fsmState.SetResult(textMessage: "Bye! See you later");
            }

            return _fsmState;
        }
    }
}
