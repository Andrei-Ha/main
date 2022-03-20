using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ConfirmBooking : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;

        public ConfirmBooking(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (_state.Propositions == null || _state.Propositions.Count < 0)
            {
                return _state;
            }

            // Confirm
            if (text == _state.Propositions[0])
            {
                // if this is a new booking
                if (_state.EditTypeEnum == EditTypeEnum.None)
                {
                    string result = "A new booking has been created.\nAll details have been sent to you by email.\nBye!";
                    if (_state.IsRecurring())
                    {
                        var response = await _httpClient.PostWebApiModel<ServiceResponse<GetRecurringBookingDto>, AddRecurringBookingDto>(
                            "booking/add/recurring", _state.AddRecurringBookingDto());

                        //temporary validation
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = "ServiceResponse or ServiceResponse.Model is null...";
                    }
                    else
                    {
                        var response = await _httpClient.PostWebApiModel<ServiceResponse<GetOneDayBookingDto>, AddBookingDto>(
                            "booking/add/one-day", _state.AddBookingDto());
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = "ServiceResponse or ServiceResponse.Model is null...";
                    }
                }
                // if this is a edited booking
                else
                {
                    _state.TextMessage = "here we have to update our booking";
                }
            }
            // Cancel
            else if (text == _state.Propositions[1])
            {
                _state.TextMessage = "You have canceled your booking.\nBye!";
            }

            _state.Propositions = new();
            _state.NextStep = "Finish";
            return _state;
        }
    }
}
