using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

/*
At current step, bot sends to user three types of booking type: one day, reccuring (repetitive), and continuous (2+ days in sequence).
Only buttons are used, no keyboard custom entering.
The previous step is CityChoice.
The next step is calendar action (booking) according to user's choice. File: DatesChoice
*/

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class OfficeChoice : StateMachineStep
    {
        private readonly TelegramBotClient _bot;
        private readonly IHttpClientFactory _httpClient;
        public OfficeChoice(IHttpClientFactory httpClient, TelegramBot telegramBot)
        {
            _bot = telegramBot.GetBot().Result;
            _httpClient = httpClient;
        }

        public override async Task<UserState> Execute(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                string? text = update.Message?.Text;
                var httpResponse = await _httpClient.GetWebApiModel<IEnumerable<OfficeGetDto>>("office", _state.User.Token);
                if (httpResponse?.Model != null)
                {
                    var office = httpResponse.Model.FirstOrDefault(p => $"{p.Name} ({p.Address})" == text);
                    if (office != null)
                    {
                        _state.OfficeId = office.Id;
                        _state.OfficeName = text!;
                        if (_state.IsOfficeReportSelected)
                        {
                            _state.BookingType = BookingTypeEnum.Continuous;
                            _state.CalendarDate = DateTime.Today;
                            _state.CallbackMessageId = await _bot.SendCalendar
                                (update, _state.CalendarDate, _state.AddTextToCalendarForReport(), _state.Adapt<RecurrencePattern>(), true);
                        }
                        else
                        {
                            _state.TextMessage = "Select booking type:";
                            _state.Propositions = new() { "One day", "Continuous", "Recurring" };
                            _state.NextStep = nameof(DatesChoice);
                        }
                    }
                }
            }
            // if Update.Type == Update.CallbackQuery
            else
            {
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

                                    _state.EndDate = selectedDate < _state.StartDate ? _state.StartDate : selectedDate;
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
                    case Constants.Ok:
                        {
                            _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                            isOkClicked = true;
                            break;
                        }
                    case Constants.Back:
                        {
                            _state.CallbackMessageId = await _bot.DeleteInlineKeyboardWithText(update);
                            return _state;
                        }
                }
                if (isOkClicked)
                {
                    // здесь у тебя уже есть все данные для формирования отчета
                    // делай запрос к БД и выводи в TextMessage
                    _state.TextMessage = "здесь можно отобразить все данные отчета";
                    _state.TextMessage += "\n\n<b>Please choose your next action!</b>";
                    _state.Propositions = new() { "Manage workplaces", "Get office report" , "Nothing" };
                    _state.NextStep = nameof(OfficeReportChoice);
                    // обнуляем введеные ранее данные
                    _state.InitRecurrencePattern();
                    _state.IsOfficeReportSelected = false;

                }
                else
                {
                    await _bot.EditCalendar(update, _state.CalendarDate, _state.AddTextToCalendarForReport(), _state.Adapt<RecurrencePattern>(), true);
                }
            }

            return _state;
        }
    }
}
