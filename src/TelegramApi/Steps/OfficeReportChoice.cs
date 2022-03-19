using Exadel.OfficeBooking.Domain.Bookings;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.DTO.ReportDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class OfficeReportChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public OfficeReportChoice(IHttpClientFactory httpClient)
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

            // Manage workplaces
            if (text == _state.Propositions[0])
            {
                _state.TextMessage = "For whom you would like to manage workplace?";
                _state.Propositions = new() { "For myself", "For other employees" };
                _state.NextStep = nameof(ManageForChoice);
            }

            // Get office report
            else if (text == _state.Propositions[1])
            {
                _state.IsOfficeReportSelected = true;
                _state.BookingType = BookingTypeEnum.Continuous;
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

            return _state;
        }
    }
}
