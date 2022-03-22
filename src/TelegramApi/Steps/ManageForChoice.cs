using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

/*
This step is available only for admin role.
It allows admins to book and manage bookings on behalf of other users.
Both, new booking and changing a booking leads to exact flow as in ordinary users' flows.
The next step depends on admin's choice: new booking or change/cancel booking.
*/

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

                // If the choice is "For myself"
                if (text == _state.Propositions[0])
                {
                    _state.TextMessage = $"Ok. What do you want to do today?";
                    _state.Propositions = new() { "Book a workplace", "Nothing", "Change or Cancel a booking" };
                    _state.NextStep = nameof(ActionChoice);
                }
                // If the choice is "For other employee"
                else if (text == _state.Propositions[1])
                {
                    var httpResponse = await _http.GetWebApiModel<IEnumerable<LoginUserDto>>("login");
                    if (httpResponse?.Model != null)
                    {
                        var dictionary = httpResponse.Model
                            .Where(u => u.Role != UserRole.Admin || u.UserId == _state.User.UserId)
                            .OrderBy(m => m.LastName)
                            .ToDictionary(k => $"{k.UserId}", v => $"{ v.LastName } { v.FirstName }");
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
                    System.Console.WriteLine($"login/{update.CallbackQuery.Data}");
                    var loginUserDto = httpResponse?.Model;
                    if (loginUserDto != null)

                    {
                        // delete inlineKeyboard and set CallbackMessageId to default value
                        _state.CallbackMessageId = await _bot.DeleteInlineKeyboard(update);
                        _state.User.UserId = loginUserDto.UserId;
                        _state.User.FirstName = loginUserDto.FirstName;
                        _state.User.LastName = loginUserDto.LastName;
                        _state.User.Email = loginUserDto.Email;
                        // !!! Token and role aren't changed
                        _state.TextMessage = $"You have selected employee with the name: <b>{_state.GetFullName()}</b>.\n What would you like to do on his behalf?";
                        _state.Propositions = new() { "Book a workplace", "Nothing", "Change or Cancel a booking" };
                        _state.NextStep = nameof(ActionChoice);
                    }
                }
                return _state;
            }
        }
    }
}
