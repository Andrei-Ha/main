using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ManageForChoice : StateMachineStep
    {
        private readonly IHttpClientFactory _http;
        private readonly TelegramBotClient _bot;

        public ManageForChoice(IHttpClientFactory httpClientFactory, TelegramBot telegramBot)
        {
            _http = httpClientFactory;
            _bot = telegramBot.GetBot().Result;
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

                // for myself
                if (text == _state.Propositions[0])
                {
                    _state.TextMessage = $"Ok. What do you want to do today?";
                    _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                    _state.NextStep = nameof(ActionChoice);
                }
                // for other employee
                else if (text == _state.Propositions[1])
                {
                    _state.IsBookForOther = true;
                    var httpResponse = await _http.GetWebApiModel<IEnumerable<LoginUserDto>>("login");
                    if (httpResponse?.Model != null)
                    {
                        var dictionary = httpResponse.Model
                            .OrderBy(m => m.LastName)
                            .ToDictionary(k => $"{ k.LastName } { k.FirstName }", v => $"{v.UserId}");
                        _state.CallbackMessageId = await _bot.SendInlineKbList(update, "Select the employee:", dictionary);
                    }

                }

                return _state;
            }
            // If  update.Type == UpdateType.CallbackQuery
            else
            {
                if (_state.CallbackMessageId == update.CallbackQuery.Message.MessageId)
                {
                    var httpResponse = await _http.GetWebApiModel<LoginUserDto>($"login/{update.CallbackQuery.Data}");
                    var loginUserDto = httpResponse?.Model;
                    if (loginUserDto != null)

                    {
                        // delete inlineKeyboard and set CallbackMessageId to default value
                        _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                        _state.User.UserId = loginUserDto.Id;
                        _state.User.FirstName = loginUserDto.FirstName;
                        _state.User.LastName = loginUserDto.LastName;
                        _state.User.Email = loginUserDto.Email;
                        // !!! Token and role don't changed
                        _state.TextMessage = $"You have selected employee with name: <b>{_state.GetFullName()}</b>.\n What do you want to do on his behalf?";
                        _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                        _state.NextStep = nameof(ActionChoice);
                    }
                }
                return _state;
            }
        }
    }
}