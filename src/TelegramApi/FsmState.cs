using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class FsmState
    {
        public long TelegramId { get; set; } = 0;

        public LoginUserDto User { get; set; } = new();
        
        public string City { get; set; }  = string.Empty;

        public Guid OfficeId { get; set; } = default;

        public BookingTypeEnum BookingType { get; set; } = BookingTypeEnum.None;

        public DateTime? DateStart { get; set; } = null;

        public DateTime? DateEnd { get; set; } = null;

        public string StepName { get; set; } = string.Empty; // nameof(Start)

        public Result Result { get; set; } = new();
    }
}
