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
    public class EditingChoise : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public EditingChoise(IHttpClientFactory httpClient)
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

            // I want to change office
            if (text == _state.Propositions[0])
            {
                _state.EditTypeEnum = EditTypeEnum.OfficeChange;
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
            // I want to change workplace in the same office
            else if (text == _state.Propositions[1])
            {
                _state.EditTypeEnum = EditTypeEnum.WorkplaceChange;
                _state.SetByeAndFinish();
            }
            // I want to change my booking dates
            else if (text == _state.Propositions[2])
            {
                _state.EditTypeEnum = EditTypeEnum.DatesChange;
                _state.SetByeAndFinish();
            }
 
            return _state;
        }
    }
}