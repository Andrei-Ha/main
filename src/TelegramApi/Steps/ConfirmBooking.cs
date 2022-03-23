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
                string failure = "<b>Booking failed!</b>\nSorry, this workspace is no longer available for your dates.\n<b>Bye!</b>";
                // if this is a new booking
                if (_state.EditTypeEnum == EditTypeEnum.None)
                {
                    string result = "A <b>new booking</b> has been <b>created</b>.\nAll details have been sent to you by email.\n<b>Bye!</b>";
                    if (_state.IsRecurring())
                    {
                        var response = await _httpClient.PostWebApiModel<ServiceResponse<GetRecurringBookingDto>, AddRecurringBookingDto>(
                            "booking/add/recurring", _state.AddRecurringBookingDto(), _state.User.Token);

                        //temporary validation
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = failure;
                    }
                    else
                    {
                        var response = await _httpClient.PostWebApiModel<ServiceResponse<GetOneDayBookingDto>, AddBookingDto>(
                            "booking/add/one-day", _state.AddBookingDto(), _state.User.Token);
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = failure;
                    }
                }
                // if this is a edited booking
                else
                {
                    string result = "A booking has been <b>edited</b>.\nAll details have been sent to you by email.\n<b>Bye!</b>";
                    if (_state.IsRecurring())
                    {
                        var response = await _httpClient.PutWebApiModel<ServiceResponse<GetRecurringBookingDto>, AddRecurringBookingDto>(
                            $"booking/update/recurring/{_state.BookingId}", _state.AddRecurringBookingDto(), _state.User.Token);

                        //temporary validation
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = failure;
                    }
                    else
                    {
                        Console.WriteLine($"booking/update/one-day/{_state.BookingId}");
                        var response = await _httpClient.PutWebApiModel<ServiceResponse<GetOneDayBookingDto>, AddBookingDto>(
                            $"booking/update/one-day/{_state.BookingId}", _state.AddBookingDto(), _state.User.Token);
                        if (response?.Model != null)
                            _state.TextMessage = response.Model.Success ? result : response.Model.Message;
                        else
                            _state.TextMessage = failure;
                    }
                }
            }
            // Cancel
            else if (text == _state.Propositions[1])
            {
                _state.TextMessage = "You have <b>canceled</b> your booking.\n<b>Bye!</b>";
            }

            _state.Propositions = new();
            _state.NextStep = "Finish";
            return _state;
        }
    }
}
