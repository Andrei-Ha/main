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
                _fsmState.Result = new Result
                {
                    TextMessage = $"Hello, {_fsmState.User.FirstName}! What do you want to do today?",
                    NextStep = nameof(ActionChoise),
                    Propositions = new string[] { "Change or Cancel a booking", "Book a workplace", "Nothing" }
                };
            }
            else
            {
                _fsmState.Result = new Result
                {
                    TextMessage = "Sorry, you can't do booking!"
                };
            }

            return _fsmState;
        }
    }
}
