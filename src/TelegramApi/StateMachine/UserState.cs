using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class UserState
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long TelegramId { get; set; } = 0;

        public LoginUserDto User { get; set; } = new();
        
        public string City { get; set; }  = string.Empty;

        public Guid OfficeId { get; set; } = default;

        public string OfficeName { get; set; } = string.Empty;

        public BookingTypeEnum BookingType { get; set; } = BookingTypeEnum.None;

        public DateTime DateStart { get; set; } = default;

        public DateTime DateEnd { get; set; } = default;

        public bool IsParkingPlace { get; set; } = false;

        public bool IsSpecifyWorkplace { get; set; } = false;

        public string NextStep { get; set; } = "Finish";

        public string TextMessage { get; set; } = "Not implemented yet";

        public List<string>? Propositions { get; set; } = new();

        public Result GetResult()
        {
            return new Result() { TextMessage = TextMessage, Propositions = Propositions };
        }

        public void SetResult(string textMessage = "Not implemented yet", List<string>? propositions = default, string nextStep = "Finish")
        {
            TextMessage = textMessage;
            Propositions = propositions;
            NextStep = nextStep;
        }

        public string Summary()
        {
            StringBuilder sb = new();
            sb.Append($"{User.FirstName} {User.LastName}, email:{User.Email}\n");
            sb.Append($"Office: {OfficeName} {City}\n");
            sb.Append($"Booking type: {BookingType.ToString()}\n");
            if (BookingType == BookingTypeEnum.OneDay)
            {
                sb.Append($"Booking date: {DateStart.ToString("dd.MM.yyyy")}\n");
            }
            if (BookingType == BookingTypeEnum.Continuous)
            {
                sb.Append($"Booking first day: {DateStart.ToString("dd.MM.yyyy")} and last day:{DateEnd.ToString("dd.MM.yyyy")}\n");
            }
            if (IsParkingPlace) 
            {
                sb.Append($"Parking place added\n"); 
            }
            /*sb.Append($"{}\n");
            sb.Append($"{}\n");
            sb.Append($"{}\n");*/
            return sb.ToString();
        }
    }    
}
