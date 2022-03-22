using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

/*
This step covers every booking period logic, such as one day, continuous and reccuring.
Previous step is OfficeChoice.
Next step is ParkingChoice.
*/

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class DatesChoice : StateMachineStep
    {
        private readonly TelegramBotClient _bot;
        private readonly IHttpClientFactory _httpClient;

        public DatesChoice(IHttpClientFactory httpClient, TelegramBot telegramBot)
        {
            _bot = telegramBot.GetBot().Result;
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                string? text = update.Message?.Text;
                _state.InitRecurrencePattern();
                if (_state.Propositions == null)
                {
                    return _state;
                }

                // One day
                if (text == _state.Propositions[0])
                {
                    _state.BookingType = BookingTypeEnum.OneDay;
                }
                // Continuous
                else if (text == _state.Propositions[1])
                {
                    _state.BookingType = BookingTypeEnum.Continuous;
                }
                // Reccuring
                else if (text == _state.Propositions[2])
                {
                    _state.BookingType = BookingTypeEnum.Recurring;
                }

                if (_state.BookingType != BookingTypeEnum.None)
                {
                    _state.CalendarDate = DateTime.Today;
                    _state.CallbackMessageId = await _bot.SendCalendar(update, _state.CalendarDate, _state.AddTextToCalendar(), _state.Adapt<RecurrencePattern>());
                }
            }
            // if Update.Type == Update.CallbackQuery
            else
            {
                if (_state.CallbackMessageId == update.CallbackQuery.Message.MessageId)
                {
                    //await _bot.EchoCallbackQuery(update);
                    string[] key = update.CallbackQuery.Data.Split('/');
                    bool isOkClicked = false;

                    switch (key[0] + "/")
                    {
                        case Constants.PickDate:
                            {
                                DateTime.TryParseExact(key[1], "dd'.'MM'.'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate);
                                if (_state.StartDate == default)
                                {
                                    _state.StartDate = selectedDate;
                                }
                                else
                                {
                                    if (_state.EndDate == default)
                                    {
                                        if ((_state.BookingType == BookingTypeEnum.Recurring && _state.Count > 0) || _state.BookingType == BookingTypeEnum.OneDay)
                                        {
                                            _state.StartDate = selectedDate;
                                        }
                                        else
                                        {
                                            _state.EndDate = selectedDate < _state.StartDate ? _state.StartDate : selectedDate;
                                        }
                                    }
                                    else
                                    {
                                        _state.StartDate = selectedDate;
                                        _state.EndDate = default;
                                    }
                                }
                                break;
                            }
                        case Constants.ChangeTo:
                            {
                                if (DateTime.TryParse(key[1], out DateTime newDate))
                                {
                                    _state.CalendarDate = newDate;
                                }
                                break;
                            }
                        case Constants.DayOfWeek:
                            {
                                if (Enum.TryParse(key[1], out WeekDays weekDays))
                                {
                                    _state.RecurringWeekDays = weekDays;
                                }
                                break;
                            }
                        case Constants.Frequency:
                            {
                                if (Enum.TryParse(key[1], out RecurringFrequency frequency))
                                {
                                    _state.Frequency = frequency;
                                }
                                break;
                            }
                        case Constants.Interval:
                            {
                                if (int.TryParse(key[1], out int incr))
                                {
                                    _state.Interval += incr;
                                }
                                break;
                            }
                        case Constants.Count:
                            {
                                if (int.TryParse(key[1], out int incr))
                                {
                                    _state.Count += incr;
                                    _state.EndDate = _state.Count > 0 ? default : _state.EndDate;
                                }
                                break;
                            }
                        case Constants.Ok:
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                isOkClicked = true;
                                break;
                            }
                        case Constants.Back:
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboardWithText(update);
                                //_state.TextMessage = "Select your booking type again:";
                                return _state;
                            }
                    }
                    if (isOkClicked)
                    {
                        if (_state.EditTypeEnum != EditTypeEnum.DatesChange)
                        {
                            _state.TextMessage = "Would you like to add parking place?";
                            _state.Propositions = new() { "yes", "no" };
                            _state.NextStep = nameof(ParkingChoice);
                        }
                        else
                        {
                            //This should be a request to the web API to check if our place can be booked for the new dates
                            bool success = false;
                            if (_state.IsRecurring())
                            {
                                var response = await _httpClient.PutWebApiModel<ServiceResponse<GetRecurringBookingDto>, AddRecurringBookingDto>(
                                    $"booking/update/recurring/{_state.BookingId}?onlyCheck=true", _state.AddRecurringBookingDto());

                                if (response?.Model != null)
                                    success = response.Model.Success;
                            }
                            else
                            {
                                var response = await _httpClient.PutWebApiModel<ServiceResponse<GetOneDayBookingDto>, AddBookingDto>(
                                    $"booking/update/one-day/{_state.BookingId}?onlyCheck=true", _state.AddBookingDto());
                                if (response?.Model != null)
                                    success = response.Model.Success;
                            }

                            if (success)
                            {
                                _state.TextMessage = _state.Summary() + "\nConfirm the booking?";
                                if (_state.EditTypeEnum != EditTypeEnum.DatesChange)
                                {
                                    _state.TextMessage += "\nThis is edited booking(from DatesChoise).";
                                }
                                _state.Propositions = new() { "confirm", "cancel" };
                                _state.NextStep = nameof(ConfirmBooking);
                            }
                            else
                            {
                                _state.TextMessage = "The requested dates are not available for booking.\n";
                                _state.TextMessage += "To end the dialogue and exit - send me <b>/Finish</b>\n";
                                _state.TextMessage += "or try again\n";
                                _state.TextMessage += "Select booking type:";
                                _state.Propositions = new() { "One day", "Continuous", "Recurring" };
                                _state.NextStep = nameof(DatesChoice);
                            }
                        }
                    }
                    else
                    {
                        await _bot.EditCalendar(update, _state.CalendarDate, _state.AddTextToCalendar(), _state.Adapt<RecurrencePattern>());
                    }
                }
            }
            
            return _state;
        }
    }
}
