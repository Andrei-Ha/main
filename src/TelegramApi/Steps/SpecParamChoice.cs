using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
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
    public class SpecParamChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;

        public SpecParamChoice(IHttpClientFactory httpClient)
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

            // Yes, I have special preferences
            if (text == _state.Propositions[0])
            {
                _state.IsSpecifyWorkplace = true;
                _state.SetResult();
            }
            // No, I can take any available workplace
            else if (text == _state.Propositions[1])
            {
                _state.IsSpecifyWorkplace = false;

                if (_state.IsRecurring)
                {
                    var recuringBooking = new AddFirstFreeWorkplaceRecuringBookingDto()
                    {
                        UserId = _state.User.UserId,
                        OfficeId = _state.OfficeId,
                        StartDate = _state.DateStart,
                        EndDate = _state.DateEnd,
                        Count = _state.Count,
                        Interval = (int)_state.Interval,
                        RecurringWeekDays = (WeekDays)_state.RecurringWeekDays,
                        Frequency = (RecurringFrequency)_state.Frequency
                    };

                    var httpResponseRecuring = await _httpClient.PostWebApiModel<WorkplaceGetDto, AddFirstFreeWorkplaceRecuringBookingDto>("booking/add/recuringfirstfree", recuringBooking);
                    _state.WorkplaceId = httpResponseRecuring.Model.Id;
                }

                var booking = new AddFirstFreeWorkplaceBookingDto()
                {
                    UserId = _state.User.UserId,
                    OfficeId = _state.OfficeId,
                    Date = _state.DateStart
                };
                var httpResponse = await _httpClient.PostWebApiModel<WorkplaceGetDto, AddFirstFreeWorkplaceBookingDto>("booking/add/recuringfirstfree", booking);
                _state.WorkplaceId = httpResponse.Model.Id;

                _state.TextMessage = _state.Summary() + "\n\nConfirm the booking?";
                _state.Propositions = new() { "confirm", "cancel" };
                _state.NextStep = nameof(Template);
            }
            return _state;
        }
    }
}
