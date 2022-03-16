﻿using Exadel.OfficeBooking.TelegramApi.Calendar;
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
                    bool isDeleted = false;

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
                        case Constants.Back:
                            {
                                await _bot.DeleteInlineKeyboardWithText(update);
                                return _state;
                            }
                    }
                    if (isDeleted)
                    {
                        _state.TextMessage = "Would you like to add parking place?";
                        _state.Propositions = new() { "yes", "no" };
                        _state.NextStep = nameof(ParkingChoice);

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
