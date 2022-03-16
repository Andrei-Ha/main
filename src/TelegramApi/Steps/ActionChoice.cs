using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ActionChoice : StateMachineStep
    {
        private readonly TelegramBotClient _bot;
        private readonly IHttpClientFactory _httpClient;
        public ActionChoice(IHttpClientFactory httpClient, TelegramBot telegramBot)
        {
            _bot = telegramBot.GetBot().Result;
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                string? text = update.Message?.Text;
                if (_state.Propositions == null)
                {
                    return _state;
                }

                // Change or Cancel a booking
                if (text == _state.Propositions[0])
                {
                    _state.SetResult();
                }

                // Book a workplace
                else if (text == _state.Propositions[1])
                {
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

                // Nothing
                else if (text == _state.Propositions[2])
                {
                    _state.SetResult(textMessage: "Bye! See you later");
                }
                // Calendar
                else if (text == _state.Propositions[3])
                {
                    // test start
                    _state.BookingType = BookingTypeEnum.Recurring;
                    _state.StartDate = default; //DateTime.Today.AddDays(2);
                    _state.EndDate = default; //DateTime.Today.AddMonths(2);
                    _state.Count = 0;
                    _state.Interval = 1;
                    //_state.RecurringWeekDays = WeekDays.Sunday;
                    //_state.Frequency = RecurringFrequency.Daily;
                    // test end
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
                    bool isDeleted = false;

                    switch (key[0] + "/")
                    {
                        case Constants.PickDate:
                            {
                                _ = DateTime.TryParse(key[1], out DateTime selectedDate);
                                if (_state.StartDate == default)
                                {
                                    _state.StartDate = selectedDate;
                                }
                                else
                                {
                                    if (_state.EndDate == default)
                                    {
                                        if (_state.BookingType == BookingTypeEnum.Recurring && _state.Count > 0)
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
                                isDeleted = true;
                                break;
                            }
                    }
                    if(!isDeleted)
                        await _bot.EditCalendar(update, _state.CalendarDate, _state.AddTextToCalendar(), _state.Adapt<RecurrencePattern>());
                }
            }

            return _state;
        }
    }
}
