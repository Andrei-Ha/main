using Exadel.OfficeBooking.TelegramApi.DTO.MapDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class FloorChoice : StateMachineStep
    {
        private readonly TelegramBotClient _bot;
        private readonly IHttpClientFactory _http;

        public FloorChoice(IHttpClientFactory httpClientFactory, TelegramBot telegramBot)
        {
            _bot = telegramBot.GetBot().Result;
            _http = httpClientFactory;
        }
        public override async Task<UserState> Execute(Update update)
        {
            string? text = update.Message?.Text;
            if (update.Type == UpdateType.Message)
            {
                if (_state.Propositions == null)
                {
                    return _state;
                }

                // Yes, I like to choose the exact floor
                if (text == _state.Propositions[0])
                {
                    Console.WriteLine($"map?{_state.Adapt<MapFilterDto>().GetQueryString()}");
                    var httpResponse = await _http.GetWebApiModel<IEnumerable<MapGetDto>>($"map?{_state.Adapt<MapFilterDto>().GetQueryString()}");
                    if (httpResponse?.Model != null)
                    {
                        var dictionary = httpResponse.Model
                            .OrderBy(m => m.FloorNumber)
                            .ToDictionary(k => $"{k.FloorNumber }:{k.Id}", v => $"{ v.FloorNumber }");
                        dictionary.Add("Back:true", "<< Back");
                        _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Select the floor:", dictionary);
                    }
                    else
                    {
                        // this office don't contain any maps
                        _state.TextMessage = "This office don't contain any maps";
                        _state.NextStep = "Finish";
                    }
                }
                // No, any floor with attributes:
                else if (text == _state.Propositions[1])
                {
                    _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Select floor attributes:", GetList());

                }
                return _state;
            }
            else // if Update.Type == CallbackQuery
            {
                string[] data = update.CallbackQuery.Data.Split(':');

                // if CallbackQuery received from "exact floor"
                if (Guid.TryParse(data[1], out Guid result))
                {
                    _state.MapId = result;
                    _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                    if (int.TryParse(data[0], out int floorNumber))
                    {
                        _state.FloorNumber = floorNumber;
                    }
                    _state.TextMessage = $"You have selected floor: <b>{data[0]}</b>.\n";
                    _state.TextMessage += "Would you like to choose the exact workplace?";
                    _state.Propositions = new()
                    {
                        "yes, I want to choose the exact workplace",
                        "no, I want to select workplace attributes"
                    };
                    _state.NextStep = nameof(WorkplaceChoice);

                }
                // if CallbackQuery received from "floor with attributes"
                else if (bool.TryParse(data[1], out bool result2))
                {
                    switch (data[0])
                    {
                        case "Kitchen":
                            {
                                _state.IsKitchenPresent = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "Meeting room":
                            {
                                _state.IsMeetingRoomPresent = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        // << Back
                        case "Back":
                            {
                                _state.IsKitchenPresent = false;
                                _state.IsMeetingRoomPresent = false;
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                break;
                            }
                        // [ OK ]
                        case "OK":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                var dictionary = GetList();
                                _state.TextMessage = string.Empty;
                                _state.TextMessage += _state.IsKitchenPresent ? $"{dictionary.ElementAt(0).Value}\n" : "";
                                _state.TextMessage += _state.IsMeetingRoomPresent ? $"{dictionary.ElementAt(1).Value}\n" : "";

                                var httpResponse = await _http.GetWebApiModel<IEnumerable<MapGetDto>>($"map?{_state.Adapt<MapFilterDto>().GetQueryString()}");
                                if (httpResponse?.Model != null)
                                {
                                    var floor = httpResponse.Model.FirstOrDefault();
                                    _state.MapId = floor != null ? floor.Id : default;
                                    _state.TextMessage += $"We have chosen the <b>{(floor != null ? floor.FloorNumber : default)}th</b> floor for you\n";
                                    _state.TextMessage += "Would you like to choose the exact workplace?";
                                    _state.Propositions = new() { "yes", "no" };
                                    _state.NextStep = nameof(WorkplaceChoice);
                                }
                                else
                                {
                                    _state.TextMessage += $"No floors with given attributes!";
                                    _state.TextMessage += "Would you like to choose the exact floor?";
                                    _state.Propositions = new()
                                    {
                                        "yes, I want to choose the exact floor",
                                        "no, I want to select floor attributes"
                                    };
                                    _state.NextStep = nameof(FloorChoice);
                                }

                                break;
                            }
                    }
                }// End if CallbackQuery received from "floor with attributes"                
                return _state;
            }
        }
        
        private Dictionary<string, string> GetList()
        {
            var dictionary = new Dictionary<string, string>();
           if (_state.IsKitchenPresent)
            {
                dictionary.Add("Kitchen:false", "Kitchen ☑");
            }
            else
            {
                dictionary.Add("Kitchen:true", "Kitchen ◻️");
            }

            if (_state.IsMeetingRoomPresent)
            {
                dictionary.Add("Meeting room:false", "Meeting room ☑");
            }
            else
            {
                dictionary.Add("Meeting room:true", "Meeting room ◻️");
            }

            dictionary.Add("OK:true", "[ OK ]");
            dictionary.Add("Back:true", "<< Back");
            return dictionary;
        }
    }
}