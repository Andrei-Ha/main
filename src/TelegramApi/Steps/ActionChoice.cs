using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
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

                // Book a workplace
                if (text == _state.Propositions[0])
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
                else if (text == _state.Propositions[1])
                {
                    _state.SetByeAndFinish();
                }

                // Change or Cancel a booking
                else if (_state.Propositions.Count == 3 && text == _state.Propositions[2])
                {
                    var httpResponse = await _httpClient.GetWebApiModel<ServiceResponse<GetBookingDto[]>>("booking", _state.User.Token);
                    if (httpResponse == null || httpResponse.Model == null)
                        return _state;
                    GetBookingDto[]? bookings = httpResponse.Model.Success ? httpResponse.Model.Data.Where(u => u.UserId == _state.User.UserId).ToArray() : null;
                    if (bookings == null || bookings.Length == 0)
                    {
                        _state.TextMessage = "Oops, you don't seem to have any bookings, want to make one?";
                        _state.Propositions = new List<string>() { "yes, I want to make one", "no, thanks" };
                        _state.NextStep = nameof(ActionChoice);
                    }
                    else
                    {
                        var dictionary = bookings
                            .OrderBy(b => b.OfficeName).ThenBy(b => b.StartDate)
                            .ToDictionary(k => $"{k.Id}", v => $"{v.Summary}");
                        var bookViewResponse = await _bot.SendBookingList(update, "Select the booking:", dictionary);
                        _state.bookViews = bookViewResponse.BookViews;
                        _state.CallbackMessageId = bookViewResponse.BackMessageId;
                    }
                }
            }
            else // if Update.Type == CallbackQuery
            {
                await _bot.EchoCallbackQuery(update);
                string[] data = update.CallbackQuery.Data.Split(':');
                switch (data[0])
                {
                    case "Check":
                        {
                            string[] key = data[1].Split('/');
                            if (bool.TryParse(key[1], out bool isChecked))
                            {
                                _state.bookViews.ForEach(b => b.IsChecked = (b.BookingId == key[0]) ? isChecked : b.IsChecked);
                                await _bot.UpdateBookinList(update, _state.bookViews.Where(b => b.BookingId == key[0]).ToList());
                            }
                            break;
                        }
                    case "Back":
                        {
                            var listId = _state.bookViews.Select(b => b.MessageId).OrderByDescending(o => o).ToList();
                            _state.bookViews = new();
                            //listId.Add(_state.CallbackMessageId);
                            _state.CallbackMessageId = await _bot.DeleteBookinList(update, listId, _state.CallbackMessageId, true);
                            _state.TextMessage = "What else would you like to do?";
                            break;
                        }
                    case "CheckAll":
                        {
                            if (bool.TryParse(data[1], out bool isAllChecked))
                            {
                                _state.bookViews.ForEach(b => b.IsChecked = isAllChecked);
                                await _bot.UpdateBackAndCanselAllBookinList(update, _state.CallbackMessageId, isAllChecked);
                            }
                            await _bot.UpdateBookinList(update, _state.bookViews.OrderByDescending(b => b.MessageId).ToList());
                            break;
                        }
                }
            }
            return _state;
        }
    }
}
