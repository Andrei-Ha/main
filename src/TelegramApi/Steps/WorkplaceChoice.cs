using Exadel.OfficeBooking.TelegramApi.DTO.MapDto;
using Exadel.OfficeBooking.TelegramApi.DTO.OfficeDto;
using Exadel.OfficeBooking.TelegramApi.DTO.WorkplaceDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Mapster;
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
    public class WorkplaceChoice : StateMachineStep
    {
        private readonly TelegramBotClient _bot;
        private readonly IHttpClientFactory _http;

        public WorkplaceChoice(IHttpClientFactory httpClientFactory, TelegramBot telegramBot)
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

                // Yes, I like to choose the exact workplace
                if (text == _state.Propositions[0])
                {
                    Console.WriteLine($"workplace?{_state.Adapt<WorkplaceFilterDto>().GetQueryString()}");
                    var httpResponse = await _http.GetWebApiModel<IEnumerable<WorkplaceGetDto>>($"workplace?{_state.Adapt<WorkplaceFilterDto>().GetQueryString()}");
                    if (httpResponse?.Model != null)
                    {
                        var dictionary = httpResponse.Model
                            .OrderBy(m => m.Name)
                            .ToDictionary(k => $"{k.Name}:{k.Id}", v => $"{ v.GetNameWithAttributes(isForButton: true)}");
                        dictionary.Add("Back:true", "<< Back");
                        dictionary.Add("BackToFloor:true", "<< back to floor selection");
                        if (dictionary.Count < 3)
                        {
                            dictionary.Remove("Back:true");
                            dictionary.Add("GoToDates:true", " << go to change dates");
                            dictionary.Add("Finish:true", "[finish my booking]");
                            _state.CallbackMessageId = await _bot.SendInlineKbList(update, "There are no free workplaces on this floor for your dates, sorry", dictionary);
                        }
                        else
                        {
                            _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Select the workplace:", dictionary);
                        }
                    }
                    else
                    {
                        // !!! unreachable code !!!
                        // this map don't contain any workplaces
                        // Go to choose floor!!!
                        _state.TextMessage = "This floor don't contain any workplaces. Bye!";
                        _state.Propositions = new();
                        _state.NextStep = "Finish";
                    }
                }
                // No, any workplace with attributes:
                else if (text == _state.Propositions[1])
                {
                    _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Specify workplace attributes, confirm selection:", GetList());

                }
                return _state;
            }
            else // if Update.Type == CallbackQuery
            {
                string[] data = update.CallbackQuery.Data.Split(':');

                // if CallbackQuery received from "exact workplace"
                if (Guid.TryParse(data[1], out Guid result))
                {
                    _state.WorkplaceId = result;
                    var httpResponseWorkplace = await _http.GetWebApiModel<WorkplaceGetDto>($"workplace/{result}", _state.User.Token);
                    _state.WorkplaceName = httpResponseWorkplace?.Model != null ? httpResponseWorkplace.Model.GetNameWithAttributes() : data[0];
                    _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                    _state.TextMessage = $"You have selected workplace: <b>{_state.WorkplaceName}</b>.\n";

                    _state.TextMessage += "\n" + _state.Summary();
                    _state.TextMessage += _state.EditTypeEnum != EditTypeEnum.None ? "\nConfirm booking change?" : "\nConfirm the booking?";
                    _state.Propositions = new() { "confirm", "cancel" };
                    _state.NextStep = nameof(ConfirmBooking);

                }
                // if CallbackQuery received from "workplace with attributes"
                else if (bool.TryParse(data[1], out bool result2))
                {
                    switch (data[0])
                    {
                        case "Next to window":
                            {
                                _state.IsNextToWindow = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "HasPC":
                            {
                                _state.HasPC = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "HasMonitor":
                            {
                                _state.HasMonitor = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "HasKeyboard":
                            {
                                _state.HasKeyboard = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "HasMouse":
                            {
                                _state.HasMouse = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        case "HasHeadset":
                            {
                                //_state.TextMessage = "Would you like to choose the exact workplace?";
                                _state.HasHeadset = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        // << Back
                        case "Back":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                _state.IsNextToWindow = false;
                                _state.HasPC = false;
                                _state.HasMonitor = false;
                                _state.HasKeyboard = false;
                                _state.HasMouse = false;
                                _state.HasHeadset = false;
                                break;
                            }
                        case "BackToFloor":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                _state.TextMessage = "Would you like to choose the exact floor?";
                                _state.Propositions = new()
                                {
                                    "yes, I want to choose the exact floor",
                                    "no, I want to select floor attributes"
                                };
                                _state.NextStep = nameof(FloorChoice);
                                _state.IsKitchenPresent = false;
                                _state.IsMeetingRoomPresent = false;
                                break;
                            }
                        case "GoToDates":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                _state.TextMessage += "Select booking type:";
                                _state.Propositions = new() { "One day", "Continuous", "Recurring" };
                                _state.NextStep = nameof(DatesChoice);
                                // Initial data
                                _state.InitRecurrencePattern();
                                _state.IsNextToWindow = false;
                                _state.HasPC = false;
                                _state.HasMonitor = false;
                                _state.HasKeyboard = false;
                                _state.HasMouse = false;
                                _state.HasHeadset = false;
                                _state.IsKitchenPresent = false;
                                _state.IsMeetingRoomPresent = false;
                                break;
                            }
                        case "Finish":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                _state.SetByeAndFinish();
                                break;
                            }
                        // OK
                        case "OK":
                            {
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                                var dictionary = GetList();
                                _state.TextMessage = string.Empty;
                                _state.TextMessage += _state.IsNextToWindow ? $"{dictionary.ElementAt(0).Value}\n" : "";
                                _state.TextMessage += _state.HasPC ? $"{dictionary.ElementAt(1).Value}\n" : "";
                                _state.TextMessage += _state.HasMonitor ? $"{dictionary.ElementAt(2).Value}\n" : "";
                                _state.TextMessage += _state.HasKeyboard ? $"{dictionary.ElementAt(3).Value}\n" : "";
                                _state.TextMessage += _state.HasMouse ? $"{dictionary.ElementAt(4).Value}\n" : "";
                                _state.TextMessage += _state.HasHeadset ? $"{dictionary.ElementAt(5).Value}\n" : "";
                                WorkplaceFilterDto workplaceFilterDto = _state.Adapt<WorkplaceFilterDto>();
                                workplaceFilterDto.IsOnlyFirstFree = true;
                                Console.WriteLine($"workplace?{workplaceFilterDto.GetQueryString()}");
                                var httpResponse = await _http.GetWebApiModel<IEnumerable<WorkplaceGetDto>>($"workplace?{workplaceFilterDto.GetQueryString()}");
                                if (httpResponse != null && httpResponse.Model != null && httpResponse?.Model.Count() != 0)
                                {
                                    var workplace = httpResponse?.Model.FirstOrDefault();
                                    var httpResponseMap = await _http.GetWebApiModel<MapGetDto>(
                                    $"map/{workplace?.MapId}", _state.User.Token);

                                    if (httpResponseMap?.Model != null)
                                    {
                                        _state.MapId = httpResponseMap.Model.Id;
                                        _state.FloorName = httpResponseMap.Model.GetNameWithAttributes();
                                    }
                                    _state.WorkplaceId = workplace != null ? workplace.Id : default;
                                    _state.WorkplaceName = workplace != null ? workplace.GetNameWithAttributes() : string.Empty;
                                    _state.TextMessage += $"We have chosen the <b>{(workplace != null ? workplace.GetNameWithAttributes() : default)}</b> workplace for you\n";
                                    _state.TextMessage += "\n" + _state.Summary();
                                    _state.TextMessage += _state.EditTypeEnum != EditTypeEnum.None ? "\nConfirm booking change?" : "\nConfirm the booking?";
                                    _state.Propositions = new() { "confirm", "cancel" };
                                    _state.NextStep = nameof(ConfirmBooking);
                                }
                                else
                                {
                                    _state.TextMessage += $"No workplaces with given attributes!\n";
                                    _state.TextMessage += "To end the dialogue and exit - send me <b>/Finish</b>\n";
                                    _state.TextMessage += "or try again\n";
                                    _state.TextMessage += "Would you like to choose the exact workplace?";
                                    _state.Propositions = new() 
                                    {
                                        "yes, I like to choose the exact workplace",
                                        "no, any workplace with attributes" 
                                    };
                                    _state.NextStep = nameof(WorkplaceChoice);
                                    // Initial attributes
                                    _state.IsNextToWindow = false;
                                    _state.HasPC = false;
                                    _state.HasMonitor = false;
                                    _state.HasKeyboard = false;
                                    _state.HasMouse = false;
                                    _state.HasHeadset = false;
                                }

                                break;
                            }
                    }
                }// End if CallbackQuery received from "workplace with attributes"                
                return _state;
            }
        }

        private Dictionary<string, string> GetList()
        {
            var dictionary = new Dictionary<string, string>();
            if (_state.IsNextToWindow)
            {
                dictionary.Add("Next to window:false", "🪟 Next to window  ☑");
            }
            else
            {
                dictionary.Add("Next to window:true", "🪟 Next to window  ◻️");
            }

            if (_state.HasPC)
            {
                dictionary.Add("HasPC:false", "💻 HasPC                     ☑");
            }
            else
            {
                dictionary.Add("HasPC:true", "💻 HasPC                     ◻️");
            }

            if (_state.HasMonitor)
            {
                dictionary.Add("HasMonitor:false", "🖥 HasMonitor         ☑");
            }
            else
            {
                dictionary.Add("HasMonitor:true", "🖥 HasMonitor         ◻️");
            }

            if (_state.HasKeyboard)
            {
                dictionary.Add("HasKeyboard:false", "⌨️ HasKeyboard      ☑");
            }
            else
            {
                dictionary.Add("HasKeyboard:true", "⌨️ HasKeyboard      ◻️");
            }

            if (_state.HasMouse)
            {
                dictionary.Add("HasMouse:false", "🐭 HasMouse             ☑");
            }
            else
            {
                dictionary.Add("HasMouse:true", "🐭 HasMouse             ◻️");
            }

            if (_state.HasHeadset)
            {
                dictionary.Add("HasHeadset:false", "🎧 HasHeadset         ☑");
            }
            else
            {
                dictionary.Add("HasHeadset:true", "🎧 HasHeadset         ◻️");
            }

            dictionary.Add("OK:true", "[ OK ]");
            dictionary.Add("Back:true", "<< Back");
            dictionary.Add("BackToFloor:true", "<< back to floor selection");
            return dictionary;
        }
        // window 🪟,PC 💻, Monitor 🖥, keyboard  ⌨️, mouse 🖰 🐭, headset 🎧, kitchen 🍽, meeting room 🚪
        // 𝟎 𝟏 𝟐 𝟑 𝟒 𝟓 𝟔 𝟕 𝟖 𝟗 
        // up ☝⬆, edit ✏, ok 🆗, cancel 🗙
        // https://unicode-table.com/en/1D7D9/
    }
}