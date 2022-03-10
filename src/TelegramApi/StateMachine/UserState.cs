using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
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

        public string OfficeName { get; set; } = string.Empty;

        public string WorkplaceName { get; set; } = string.Empty;

        public Guid OfficeId { get; set; } = default;

        public Guid MapId { get; set; } = default;

        public Guid WorkplaceId { get; set; } = default;

        public BookingTypeEnum BookingType { get; set; } = BookingTypeEnum.None;

        public DateTime DateStart { get; set; } = default;

        public DateTime DateEnd { get; set; } = default;
        
        public bool? IsEndDateGiven { get; set; }

        public bool IsRecurring { get; set; }

        public int? Count { get; set; }
        public bool? IsCountGiven { get; set; }
        public int? Interval { get; set; }
        public bool? IsIntervalGiven { get; set; }
        public WeekDays? RecurringWeekDays { get; set; }
        public RecurringFrequency? Frequency { get; set; }
        public bool? IsRecurringFrequencyWeekly { get; set; }
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
            sb.Append($"Workplace : <b>{WorkplaceName}</b>\n");
            sb.Append($"Booking type: <b>{BookingType}</b>\n");
            if (BookingType == BookingTypeEnum.OneDay)
            {
                sb.Append($"Booking date: <b>{DateStart:dd.MM.yyyy}</b>\n");
            }
            if (BookingType == BookingTypeEnum.Continuous)
            {
                sb.Append($"Booking first day: <b>{DateStart:dd.MM.yyyy}</b> and last day: <b>{DateEnd:dd.MM.yyyy}</b>\n");
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
