using Exadel.OfficeBooking.TelegramApi.Steps;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public abstract class StateMachineStep
    {
        public FsmState State { get; set; } = null!;

        public abstract FsmState Execute(Update update, FsmState fsmState);
    }

    public class Result
    {
        public string TextMessage { get; set; } = "Not implemented yet";

        public string NextStep { get; set; } = "Finish";

        public string[] Propositions = System.Array.Empty<string>();
    }
}
