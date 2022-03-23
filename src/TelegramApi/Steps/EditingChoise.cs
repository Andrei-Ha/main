using Exadel.OfficeBooking.TelegramApi.DTO.MapDto;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto;
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
        private readonly IHttpClientFactory _http;
        public EditingChoise(IHttpClientFactory httpClient)
        {
            _http = httpClient;
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
                var httpResponse = await _http.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
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
                _state.IsOnlyFirstFree = false;
                _state.TextMessage = "Would you like to choose the exact floor?";
                _state.Propositions = new()
                {
                    "yes, I want to choose the exact floor",
                    "no, I want to select floor attributes"
                };
                _state.NextStep = nameof(FloorChoice);
            }
            // I want to change my booking dates
            else if (text == _state.Propositions[2])
            {
                // fill the FloorName and WorkplaceName
                var httpResponseWorkplace = await _http.GetWebApiModel<WorkplaceGetDto>($"workplace/{_state.WorkplaceId}", _state.User.Token);
                if (httpResponseWorkplace?.Model != null)
                {
                    var httpResponseMap = await _http.GetWebApiModel<MapGetDto>($"map/{httpResponseWorkplace.Model.MapId}", _state.User.Token);
                    _state.FloorName = httpResponseMap?.Model != null ? httpResponseMap.Model.GetNameWithAttributes() : _state.FloorName;
                    _state.WorkplaceName = httpResponseWorkplace.Model.GetNameWithAttributes();
                }
                _state.EditTypeEnum = EditTypeEnum.DatesChange;
                _state.TextMessage = "Select booking type:";
                _state.Propositions = new() { "One day", "Continuous", "Recurring" };
                _state.NextStep = nameof(DatesChoice);
            }
 
            return _state;
        }
    }
}
