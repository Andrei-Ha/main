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
                if (_state.IsRecurring)
                {
                    AddRecurringBookingDto newBooking = new AddRecurringBookingDto
                    {
                        UserId = _state.User.UserId,
                        WorkplaceId = _state.WorkplaceId,
                        StartDate = _state.StartDate,
                        EndDate = _state.EndDate,
                        Count = _state.Count,
                        Interval = _state.Interval ?? 1,
                        RecurringWeekDays = _state.RecurringWeekDays ?? 0,
                        Frequency = _state.Frequency ?? RecurringFrequency.Daily
                    };
                    var response = await _httpClient.PostWebApiModel<ServiceResponse<GetRecurringBookingDto>, AddRecurringBookingDto>(
                        "booking/add/recurring", newBooking);
                    
                    //temporary validation
                    if (!response.Model.Success)
                    {
                        _state.TextMessage = response.Model.Message;
                        _state.Propositions = new List<string>();
                        _state.NextStep = "Finish";
                        return _state;
                    }
                }
                else
                {
                    AddBookingDto newBooking = new AddBookingDto
                    {
                        UserId = _state.User.UserId,
                        WorkplaceId = _state.WorkplaceId,
                        Date = _state.StartDate
                    };
                    var response = await _httpClient.PostWebApiModel<ServiceResponse<GetOneDayBookingDto>, AddBookingDto>(
                        "booking/add/one-day", newBooking);
                    if (!response.Model.Success)
                    {
                        _state.TextMessage = response.Model.Message;
                        _state.Propositions = new List<string>();
                        _state.NextStep = "Finish";
                        return _state;
                    }
                }
                
                _state.TextMessage = "A new booking has been created.\nAll details have been sent to you by email.\nBye!";
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
