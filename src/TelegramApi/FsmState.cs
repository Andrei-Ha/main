using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class FsmState
    {
        public long TelegramId { get; set; } = 0;

        public LoginUserDto? User { get; set; } 
        
        public string City { get; set; }  = string.Empty;

        public string StepName { get; set; } = string.Empty; // nameof(Start)

        public Result Result { get; set; } = new();
    }
}
