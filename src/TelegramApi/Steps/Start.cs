using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        private readonly IHttpClientFactory _httpClient;
        public Start(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public override async Task<FsmState> Execute(Update update)
        {
            var httpResponse = await _httpClient.PostWebApiModel<LoginUserDto, string>("login", _state.TelegramId.ToString());
            var user = httpResponse?.Model;
            if ( user != null)
            {
                _state.User = user;
                _state.TextMessage = $"Hello, {_state.User.FirstName}! What do you want to do today?";
                _state.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                _state.NextStep = nameof(ActionChoice);
            }
            else
            {
                _state.SetResult(textMessage: "Sorry, you can't do booking!");
                // or
                //_state.TextMessage = "Sorry, you can't do booking!";
                //_state.Propositions = default;
                //_state.NextStep = "Finish";
            }

            return _state;
        }
    }
}
