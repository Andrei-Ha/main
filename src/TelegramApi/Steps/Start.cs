using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System.Net.Http;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class Start : StateMachineStep
    {
        public Start(IHttpClientFactory http) : base(http)
        {
        }

        public override FsmState Execute(Update update, FsmState fsmState)
        {
            var loginUserDto = Login(fsmState.TelegramId).Result;
            if (loginUserDto == null)
            {
                fsmState.Result = new Result
                {
                    TextMessage = "Sorry, you can't do booking!"
                };
            }
            else
            {
                fsmState.User = loginUserDto;
                fsmState.Result = new Result
                {
                    TextMessage = $"Hello, {loginUserDto.FirstName}! What do you want to do today?",
                    NextStep = nameof(ActionChoise),
                    Propositions = new string[] { "Change or Cancel a booking", "Book a workplace", "Nothing" }
                };
            }

            return fsmState;
        }
    }
}
