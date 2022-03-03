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
                _fsmState.TextMessage = $"Hello, {_fsmState.User.FirstName}! What do you want to do today?";
                _fsmState.Propositions = new() { "Change or Cancel a booking", "Book a workplace", "Nothing" };
                _fsmState.NextStep = nameof(ActionChoice);
            }
            else
            {
                _fsmState.SetResult(textMessage: "Sorry, you can't do booking!");
                // or
                //_fsmState.TextMessage = "Sorry, you can't do booking!";
                //_fsmState.Propositions = default;
                //_fsmState.NextStep = "Finish";
            }

            return _fsmState;
        }
    }
}
