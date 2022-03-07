using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Text;
using Exadel.OfficeBooking.TelegramApi.DTO;

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
        
        public bool? IsEndDateGiven { get; set; }

        public bool IsParkingPlace { get; set; } = false;
        
        public int? Count { get; set; }

        public bool? IsCountGiven { get; set; }

        public int? Interval { get; set; }

        public bool? IsIntervalGiven { get; set; }
        
        public WeekDays? RecurringWeekDays { get; set; }
        
        public RecurringFrequency? Frequency { get; set; }
        
        public bool? IsRecurringFrequencyWeekly { get; set; }
        
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

            if (BookingType == BookingTypeEnum.Recurring)
            {
                string appendStr = $"Booking first day: {DateStart.ToString("dd.MM.yyyy")}";
                if (IsEndDateGiven != null && (bool) IsEndDateGiven)
                    appendStr += $"\nBooking last day: {DateEnd.ToString("dd.MM.yyyy")}";
                appendStr += $"\nFrequency: {Frequency.ToString()}";
                if (Frequency == RecurringFrequency.Weekly)
                    appendStr += $"\nWeekdays: {RecurringWeekDays}";
                if (IsIntervalGiven != null && (bool) IsIntervalGiven)
                    appendStr += $"\nInterval is: {Interval}";
                if (IsCountGiven != null && (bool) IsCountGiven)
                    appendStr += $"\nCount is: {Count}";
                
                sb.Append(appendStr);
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
