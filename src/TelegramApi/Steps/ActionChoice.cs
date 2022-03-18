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
                        var dictionary = bookings.OrderBy(b => b.OfficeName).ThenBy(b => b.StartDate)
                            .ToDictionary(k => $"booking:{k.Id}", v => $"{v.Summary}");
                        //List<int> listOfIdMessages = await _bot.SendBookingList(update, "Select the booking:", dictionary);
                        _state.CallbackMessageId = await _bot.SendBookingList(update, "Select the booking:", dictionary);
                    }
                }
            }
            else // if Update.Type == CallbackQuery
            {
                string[] data = update.CallbackQuery.Data.Split(':');
                switch (data[0])
                {
                    case "Back":
                        {
                            _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                            _state.TextMessage = "What else would you like to do?";
                            break;
                        }
                }
            }
            return _state;
        }
    }
}
