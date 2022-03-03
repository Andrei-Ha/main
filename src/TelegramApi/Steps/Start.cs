using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        public Start(IHttpClientFactory http) : base(http)
        {
        }

        public override async Task<FsmState> Execute(Update update)
        {
            if (await Login())
            {
                _fsmState.Result.TextMessage = $"Hello, {_fsmState.User.FirstName}! What do you want to do today?";
                _fsmState.Result.NextStep = nameof(ActionChoise);
                _fsmState.Result.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
            }
            else
            {
                _fsmState.Result.TextMessage = "Sorry, you can't do booking!";
                _fsmState.Result.NextStep = "Finish";
                _fsmState.Result.Propositions = new();
            }

            return _fsmState;
        }
    }
}
