using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

/*
    The first step of a bot evaluates user's role and lets user to decide for what purpose, bot is going to be used.
    When user authorizes, the system gets user's role from database, and if user is admin it has additional 1 action:
      1.Is admin wants to manage bookings of employees or book for him/herself.
    Then for every user actions and buttons are the same: choosing either new booking, modifying or doing nothing.
    The next step is CityChoice.
*/

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public Start(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task<UserState> Execute(Update update)
        {
            var httpResponse = await _httpClient.PostWebApiModel<LoginUserDto, string>("login", _state.TelegramId.ToString());
            var user = httpResponse?.Model;
            if ( user != null)
            {
                _state.User = user;
                if (_state.User.Role == UserRole.Admin || _state.User.Role == UserRole.Manager)
                {
                    _state.TextMessage = $"Hello, <b>{_state.User.FirstName}</b>! Your role is <b>{user.Role}</b>.\n For whom you would like to manage workplace?";
                    _state.Propositions = new() { "For myself", "For other employees" };
                    _state.NextStep = nameof(ManageForChoice);
                }
                else
                {
                    _state.TextMessage = $"Hello, <b>{_state.User.FirstName}</b>! What would you like to do today?";
                    _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                    _state.NextStep = nameof(ActionChoice);
                }
            }
            else
            {
                _state.SetResult(textMessage: "Sorry, you cannot book.");
                // or
                //_state.TextMessage = "Sorry, you can't do booking!";
                //_state.Propositions = default;
                //_state.NextStep = "Finish";
            }

            return _state;
        }
    }
}
