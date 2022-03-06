using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
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
    public class ManageForChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _http;

        public ManageForChoice(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory;
        }

        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null)
            {
                return _state;
            }

            // for myself
            if (text == _state.Propositions[0])
            {
                _state.TextMessage = $"Ok. What do you want to do today?";
                _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                _state.NextStep = nameof(ActionChoice);
            }
            // for other employee
            else if (text == _state.Propositions[1])
            {
                _state.IsBookForOther = true;
                var httpResponse = await _http.GetWebApiModel<IEnumerable<LoginUserDto>>("login");
                if (httpResponse?.Model != null)
                {
                    _state.TextMessage = "Select the employee:";
                    _state.Propositions = httpResponse.Model.Select(x => $"{x.UserId}, {x.FirstName}, {x.LastName}").ToList();
                    _state.NextStep = "Finish";
                }

            }

            return _state;
        }
    }
}