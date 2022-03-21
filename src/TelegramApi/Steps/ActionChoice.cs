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
                        _state.BookViews = bookViewResponse.BookViews;
                        _state.CallbackMessageId = bookViewResponse.BackMessageId;
                    }
                }
            }
            else // if Update.Type == CallbackQuery
            {
                //await _bot.EchoCallbackQuery(update);
                string[] data = update.CallbackQuery.Data.Split(':');
                switch (data[0])
                {
                    case "Change":
                        {
                            if (Guid.TryParse(data[1], out Guid bookingId))
                            {
                                var bookView = _state.BookViews.Where(b => b.BookingId == data[1]).FirstOrDefault();
                                if (bookView != null)
                                {
                                    _state.BookViews.Remove(bookView);
                                    _state.CallbackMessageId = await _bot.DeleteBookinList
                                        (
                                            update,
                                            _state.BookViews.Select(m => m.MessageId).OrderByDescending(m => m).ToList(),
                                            _state.CallbackMessageId
                                        );
                                    await _bot.DeleteBookinList(update, new List<int> { bookView.MessageId }, 0, true);
                                    _state.BookingId = bookingId;
                                    _state.TextMessage = "What would you like to change?";
                                    _state.Propositions = new()
                                    {
                                        "I want to change office",
                                        "I want to change workplace in the same office",
                                        "I want to change my booking dates"
                                    };
                                    _state.NextStep = nameof(EditingChoise);
                                }
                            }
                            break;
                        }
                    case "Cancel":
                        {
                            if (Guid.TryParse(data[1], out Guid bookingId))
                            {
                                //delete one booking
                                var responseDelete = await _httpClient.DeleteWebApiModel<ServiceResponse<GetBookingDto[]>>
                                        ($"booking/delete/{bookingId.ToString()}", _state.User.Token);
                                
                                if (responseDelete?.Model != null && responseDelete.Model.Success)// If booking endpoint returned OK
                                {
                                    var bookViewToDel = _state.BookViews.Where(b => b.BookingId == data[1]).FirstOrDefault();
                                    if (bookViewToDel != null)
                                    {
                                        _state.BookViews.Remove(bookViewToDel);
                                        await _bot.DeleteBookinList(update, new List<int> { bookViewToDel.MessageId });
                                    }
                                }
                            }
                            break;
                        }
                    case "CancelChecked":
                        {
                            var BookViewsToDel = _state.BookViews.Where(b => b.IsChecked == true).ToList();
                            _state.BookViews.RemoveAll(b => b.IsChecked == true);
                            if (BookViewsToDel.Any())
                            {
                                string guidsToDel = BookViewsToDel.Select(g => g.BookingId).Aggregate((i, j) => $"{i};" + j);
                                //delete all bookings with given ids
                                var responseDelete = await _httpClient.DeleteWebApiModel<ServiceResponse<GetBookingDto[]>>
                                    ($"booking/delete/{guidsToDel}", _state.User.Token);

                                if (responseDelete?.Model != null && responseDelete.Model.Success) // If booking endpoint returned OK
                                {
                                    await _bot
                                        .DeleteBookinList(update, BookViewsToDel.Select(m => m.MessageId).OrderByDescending(m => m).ToList());
                                }
                            }
                            break;
                        }
                    case "Check":
                        {
                            string[] key = data[1].Split('/');
                            if (bool.TryParse(key[1], out bool isChecked))
                            {
                                _state.BookViews.ForEach(b => b.IsChecked = (b.BookingId == key[0]) ? isChecked : b.IsChecked);
                                await _bot.UpdateBookinList(update, _state.BookViews.Where(b => b.BookingId == key[0]).ToList());
                            }
                            break;
                        }
                    case "CheckAll":
                        {
                            List<BookView> BookViewsToChange = new();
                            if (bool.TryParse(data[1], out bool isAllChecked))
                            {
                                for(int i = 0; i < _state.BookViews.Count; i++)
                                {
                                    if(_state.BookViews[i].IsChecked != isAllChecked)
                                    {
                                        _state.BookViews[i].IsChecked = isAllChecked;
                                        BookViewsToChange.Add(_state.BookViews[i]);
                                    }
                                }
                                //_state.BookViews.ForEach(b => b.IsChecked = isAllChecked);
                                await _bot.UpdateBackAndCanselAllBookinList(update, _state.CallbackMessageId, isAllChecked);
                            }

                            await _bot.UpdateBookinList(update, BookViewsToChange.OrderByDescending(b => b.MessageId).ToList());
                            break;
                        }
                    case "Back":
                        {
                            var listId = _state.BookViews.Select(b => b.MessageId).OrderByDescending(o => o).ToList();
                            _state.BookViews = new();
                            _state.CallbackMessageId = await _bot.DeleteBookinList(update, listId, _state.CallbackMessageId, true);
                            _state.TextMessage = "What else would you like to do?";
                            break;
                        }
                }
            }
            return _state;
        }
    }
}
