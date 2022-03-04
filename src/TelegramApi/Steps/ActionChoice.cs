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
        private readonly IHttpClientFactory _httpClient;
        public ActionChoice(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // Change or Cancel a booking
            if (text == _state.Propositions[0]) 
            {
                _state.SetResult();
            }

            // Book a workplace
            else if (text == _state.Propositions[1])
            {
                var httpResponse = await _httpClient.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
                IEnumerable<OfficeGetDto>? offices = httpResponse?.Model;
                if (offices != null)
                {
                    _state.TextMessage = "Select a city:";
                    _state.Propositions = offices.Select(o => o.City).OrderBy(p => p).Distinct().ToList();
                    _state.NextStep = nameof(CityChoice);
                }
                else
                {
                    _state.TextMessage = $"\nStatusCode: {httpResponse?.StatusCode.ToString()}";
                }
            }

            // Nothing
            else if (text == _state.Propositions[2])
            {
                _state.SetResult(textMessage: "Bye! See you later");
            }

            return _state;
        }
    }
}
