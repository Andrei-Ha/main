using Exadel.OfficeBooking.TelegramApi.StateMachine;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class FsmState
    {
        public long TelegramId { get; set; } = 0;
        
        public string StepName { get; set; } = string.Empty; // nameof(Start)

        public Result Result { get; set; } = new();
    }
}
