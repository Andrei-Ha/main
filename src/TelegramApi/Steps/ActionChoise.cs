using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ActionChoise : StateMachineStep
    {
        public override FsmState Execute(Update update, FsmState fsmState)
        {
            string? text = update.Message?.Text;
            // Change or Cancel a booking
            if (text == fsmState.Result.Propositions[0]) 
            {
                fsmState.Result = new Result
                {

                };
            }

            // Book a workplace
            else if (text == fsmState.Result.Propositions[1])
            {
                fsmState.Result = new Result
                {

                };
            }

            // Nothing
            else if (text == fsmState.Result.Propositions[2])
            {
                fsmState.Result = new Result
                {
                    TextMessage = "Bye! See you later",
                    //NextStep = "Finish"
                };
            }

            return fsmState;
        }
    }
}
