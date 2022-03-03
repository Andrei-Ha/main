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
    public class OfficeChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public OfficeChoice(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            var httpResponse = await _httpClient.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
            if (httpResponse?.Model != null)
            {
                var office = httpResponse.Model.FirstOrDefault(p => $"{p.Name} ({p.Address})" == text);
                if (office != null)
                {
                    _state.OfficeId= office.Id;
                    _state.OfficeName = text!;
                    _state.TextMessage = "Select booking type:";
                    _state.Propositions = new() {"One day", "Continuous", "Recurring"};
                    _state.NextStep = nameof(DatesChoice);
                }
            }
            return _state;
        }
    }
}