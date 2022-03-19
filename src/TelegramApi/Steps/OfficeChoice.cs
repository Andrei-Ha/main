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
At current step, bot sends to user three types of booking type: one day, reccuring (repetitive), and continuous (2+ days in sequence).
Only buttons are used, no keyboard custom entering.
The previous step is CityChoice.
The next step is calendar action (booking) according to user's choice. File: DatesChoice
*/

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
                    if (_state.IsOfficeReportSelected)
                    {
                        _state.TextMessage = "Select dates:";
                        _state.Propositions = new() {  };
                        //_state.CallbackMessageId = await _bot.SendCalendar(update, _state.CalendarDate, _state.AddTextToCalendar(), _state.Adapt<RecurrencePattern>());
                    }
                    _state.TextMessage = "Select booking type:";
                    _state.Propositions = new() {"One day", "Continuous", "Recurring"};
                    _state.NextStep = nameof(DatesChoice);
                }
            }
            return _state;
        }
    }
}
