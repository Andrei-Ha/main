using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

/*
Step send to user available in database cities to choose.
Only buttons are used, no keyboard custom entering.
The previous step is Start.
The next step is OfficeChoice.
*/

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class CityChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public CityChoice(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            var httpResponse = await _httpClient.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
            if (httpResponse?.Model != null)
            {
                var propositions = httpResponse.Model
                    .Where(p => p.City == text)
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name} ({p.Address})")
                    .ToList();
                if (propositions != null)
                {
                    _state.City = text!;
                    _state.TextMessage = "Select the office:";
                    _state.Propositions = propositions;
                    _state.NextStep = nameof(OfficeChoice);
                }
            }

            return _state;
        }
    }
}
