using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

/*
At this step, user can specify floor and/or choose the exact place at office for booking.
*/

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

            // If the choice is "Yes, I have special preferences"
            if (text == _state.Propositions[0])
            {
                _state.IsSpecifyWorkplace = true;
                _state.TextMessage = "Would you like to choose the exact floor?";
                _state.Propositions = new() { "yes", "no" };
                _state.NextStep = nameof(FloorChoice);
            }
            // If the choice is "No, I can take any available workplace"
            else if (text == _state.Propositions[1])
            {
                _state.IsSpecifyWorkplace = false;

                if (_state.IsRecurring)
                {
                    var recuringBooking = new GetFirstFreeWorkplaceForRecuringBookingDto()
                    {
                        UserId = _state.User.UserId,
                        OfficeId = _state.OfficeId,
                        StartDate = _state.StartDate,
                        EndDate = _state.EndDate,
                        Count = _state.Count,
                        Interval = _state.Interval ?? 1,
                        RecurringWeekDays = _state.RecurringWeekDays ?? 0,
                        Frequency = _state.Frequency ?? 0
                    };

                    var httpResponseRecuring = await _httpClient
                        .PostWebApiModel<WorkplaceGetDto, GetFirstFreeWorkplaceForRecuringBookingDto>(
                        "booking/get/recuringfirstfree", recuringBooking);

                    _state.WorkplaceId = httpResponseRecuring.Model.Id;
                    _state.WorkplaceName = httpResponseRecuring.Model.Name;
                }
                else
                {
                    var booking = new GetFirstFreeWorkplaceForBookingDto()
                    {
                        UserId = _state.User.UserId,
                        OfficeId = _state.OfficeId,
                        Date = _state.StartDate
                    };

                    var httpResponse = await _httpClient
                        .PostWebApiModel<WorkplaceGetDto, GetFirstFreeWorkplaceForBookingDto>(
                        "booking/get/firstfree", booking);

                    _state.WorkplaceId = httpResponse.Model.Id;
                    _state.WorkplaceName = httpResponse.Model.Name;
                }

                _state.TextMessage = _state.Summary() + "\nConfirm the booking?";
                _state.Propositions = new() { "confirm", "cancel" };
                _state.NextStep = nameof(ConfirmBooking);
            }
            return _state;
        }
    }
}
