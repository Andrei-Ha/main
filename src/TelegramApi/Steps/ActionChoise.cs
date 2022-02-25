using Exadel.OfficeBooking.TelegramApi.StateMachine;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class ActionChoise : StateMachineStep
    {
        public override FsmState Execute(Update update, FsmState fsmState)
        {
            System.Console.WriteLine("ActionChoise");
            return fsmState;
        }
    }
}
