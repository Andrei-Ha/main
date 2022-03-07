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

        public bool IsBookForOther = false;
        
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

        public int CallbackMessageId { get; set; } = 0;

        public Result GetResult()
        {
            return new Result() { TextMessage = TextMessage, Propositions = Propositions, IsSendMessage = CallbackMessageId == 0 };
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
            sb.Append(GetFullName() + "\n");
            sb.Append($"Email: {User.Email}" + "\n");
            sb.Append($"Office: <b>{OfficeName} {City}</b>\n");
            sb.Append($"Booking type: <b>{BookingType}</b>\n");
            if (BookingType == BookingTypeEnum.OneDay)
            {
                sb.Append($"Booking date: <b>{DateStart:dd.MM.yyyy}</b>\n");
            }
            if (BookingType == BookingTypeEnum.Continuous)
            {
                sb.Append($"Booking first day: <b>{DateStart:dd.MM.yyyy}</b> and last day: <b>{DateEnd:dd.MM.yyyy}</b>\n");
            }
            if (IsParkingPlace) 
            {
                sb.Append($"Parking place <b>added</b>\n"); 
            }

            return sb.ToString();
        }

        public string GetFullName() 
        {
            return $"<b>{User.LastName} {User.FirstName}</b>"; 
        }
    }    
}
