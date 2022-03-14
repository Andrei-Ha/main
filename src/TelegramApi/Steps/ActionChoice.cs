using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
                    _state.CalendarDate = DateTime.Today;
                    _state.CallbackMessageId = await _bot.SendCalendar(update, _state.CalendarDate);
                }
            }
            // if Update.Type == Update.CallbackQuery
            else
            {
                if (_state.CallbackMessageId == update.CallbackQuery.Message.MessageId)
                {
                    await _bot.EchoCallbackQuery(update);
                    string[] key = update.CallbackQuery.Data.Split('/');
                    Console.WriteLine(key[0]);
                    Console.WriteLine(Constants.Close);
                    if (key[0] == Constants.Close.Trim('/'))
                    {
                        Console.WriteLine("in if");
                        _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                    }
                    else if(key[0] == Constants.ChangeTo.Trim('/'))
                    {
                        if (DateTime.TryParse(key[1], out DateTime newDate))
                        {
                            _state.CalendarDate = newDate;
                        }

                        await _bot.EditCalendar(update, _state.CalendarDate);
                    }
                }
            }

            return _state;
        }
    }
}
