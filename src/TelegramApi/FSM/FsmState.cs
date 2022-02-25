using Exadel.OfficeBooking.TelegramApi.FSM.Steps;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public class FsmState
    {
        public long ChatId { get; set; }

        public StepsNamesEnum StepName { get; set; }
    }
}
