using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        public override async Task<FsmState> Execute(Update update)
        {
            if (true)
            {
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
