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
                            .ToDictionary(k => $"{k.Name}:{k.Id}", v => $"{ v.GetNameWithAttributes()}");
                        dictionary.Add("Back:true", "<< Back");
                        _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Select the workplace:", dictionary);
                    }
                    else
                    {
                        // this map don't contain any workplaces
                        _state.TextMessage = "This floor don't contain any workplaces";
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

                // if CallbackQuery received from "exact floor"
                if (Guid.TryParse(data[1], out Guid result))
                {
                    _state.WorkplaceId = result;
                    _state.WorkplaceName = data[0];
                    _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                    _state.TextMessage = $"You have selected workplace: <b>{data[0]}</b>.\n";

                    _state.TextMessage += "\n" + _state.Summary() + "\nConfirm the booking?";
                    if (_state.EditTypeEnum != EditTypeEnum.None)
                    {
                        _state.TextMessage += "\nThis is edited booking(from WorkplaceChoise).";
                    }
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
                                _state.TextMessage = "Would you like to choose the exact workplace?";
                                _state.HasHeadset = result2;
                                await _bot.EditInlineKbList(update, GetList());
                                break;
                            }
                        // << Back
                        case "Back":
                            {
                                _state.IsNextToWindow = false;
                                _state.HasPC = false;
                                _state.HasMonitor = false;
                                _state.HasKeyboard = false;
                                _state.HasMouse = false;
                                _state.HasHeadset = false;
                                _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
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

                                Console.WriteLine($"workplace?{_state.Adapt<WorkplaceFilterDto>().GetQueryString()}");
                                var httpResponse = await _http.GetWebApiModel<IEnumerable<WorkplaceGetDto>>($"workplace?{_state.Adapt<WorkplaceFilterDto>().GetQueryString()}");
                                if (httpResponse?.Model != null/* && httpResponse?.Model.Count() != 0*/)
                                {
                                    var workplace = httpResponse.Model.FirstOrDefault();
                                    _state.WorkplaceId = workplace != null ? workplace.Id : default;
                                    _state.WorkplaceName = workplace != null ? workplace.GetNameWithAttributes() : string.Empty;
                                    _state.TextMessage += $"We have chosen the <b>{(workplace != null ? workplace.GetNameWithAttributes() : default)}</b> workplace for you\n";
                                    _state.TextMessage += "\n" + _state.Summary() + "\nConfirm the booking?";
                                    _state.Propositions = new() { "confirm", "cancel" };
                                    _state.NextStep = nameof(ConfirmBooking);
                                }
                                else
                                {
                                    _state.TextMessage += $"No workplaces with given attributes!";
                                    _state.TextMessage = "Would you like to choose the exact workplace?";
                                    _state.Propositions = new() { "yes", "no" };
                                    _state.NextStep = nameof(WorkplaceChoice);
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
            return dictionary;
        }
        // window 🪟,PC 💻, Monitor 🖥, keyboard  ⌨️, mouse 🖰 🐭, headset 🎧, kitchen 🍽, meeting room 🚪
        // 𝟎 𝟏 𝟐 𝟑 𝟒 𝟓 𝟔 𝟕 𝟖 𝟗 
        // up ☝⬆, edit ✏, ok 🆗, cancel 🗙
        // https://unicode-table.com/en/1D7D9/
    }
}