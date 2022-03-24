using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.DTO.MapDto;
using Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Mapster;
using System;
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
                _state.IsOnlyFirstFree = false;
                _state.TextMessage = "Would you like to choose the exact floor?";
                _state.Propositions = new()
                {
                    "yes, I want to choose the exact floor",
                    "no, I want to select floor attributes"
                };
                _state.NextStep = nameof(FloorChoice);
            }
            // If the choice is "No, I can take any available workplace"
            else if (text == _state.Propositions[1])
            {
                _state.IsOnlyFirstFree = true;

                Console.WriteLine(_state.Adapt<WorkplaceFilterDto>().GetQueryString());
                var httpResponseWorkplace = await _httpClient.GetWebApiModel<WorkplaceGetDto[]>(
                    $"workplace?{_state.Adapt<WorkplaceFilterDto>().GetQueryString()}",
                    _state.User.Token);

                if (httpResponseWorkplace?.Model != null && httpResponseWorkplace?.Model.Length != 0)
                {
                    var httpResponseMap = await _httpClient.GetWebApiModel<MapGetDto>(
                    $"map/{httpResponseWorkplace.Model[0].MapId}",
                    _state.User.Token);

                    if (httpResponseMap?.Model != null)
                    {
                        _state.MapId = httpResponseMap.Model.Id;
                        _state.FloorName = httpResponseMap.Model.GetNameWithAttributes();
                    }
                    
                    _state.WorkplaceId = httpResponseWorkplace.Model[0].Id;
                    _state.WorkplaceName = httpResponseWorkplace.Model[0].GetNameWithAttributes();
                    _state.TextMessage = _state.Summary();
                    _state.TextMessage += _state.EditTypeEnum != EditTypeEnum.None ? "\nConfirm booking change?" : "\nConfirm the booking?";
                    _state.Propositions = new() { "confirm", "cancel" };
                    _state.NextStep = nameof(ConfirmBooking);
                }
                else
                {
                    if (_state.EditTypeEnum == EditTypeEnum.OfficeChange)
                    {
                        _state.SetByeAndFinish();
                        _state.TextMessage = "The office you requested has no available workplaces for your dates, sorry.\n" + _state.TextMessage; ;
                    }
                    else
                    {
                        _state.TextMessage = "There are no available workplaces for the dates you requested.\n";
                        _state.TextMessage += "To end the dialogue and exit - send me <b>/Finish</b>\n";
                        _state.TextMessage += "or try again\n";
                        _state.TextMessage += "Select booking type:";
                        _state.Propositions = new() { "One day", "Continuous", "Recurring" };
                        _state.NextStep = nameof(DatesChoice);
                    }
                }
            }
            return _state;
        }
    }
}
