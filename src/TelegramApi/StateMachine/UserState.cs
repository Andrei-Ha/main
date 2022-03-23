using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace Exadel.OfficeBooking.TelegramApi
{
    public class UserState
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public long TelegramId { get; set; } = 0;

        public LoginUserDto User { get; set; } = new();
        
        public string City { get; set; }  = string.Empty;

        public string OfficeName { get; set; } = string.Empty;

        public string FloorName { get; set; } = string.Empty;

        public string WorkplaceName { get; set; } = string.Empty;

        public Guid OfficeId { get; set; } = default;

        public Guid MapId { get; set; } = default;

        public Guid WorkplaceId { get; set; } = default;

        public BookingTypeEnum BookingType { get; set; } = BookingTypeEnum.None;

        public DateTime StartDate { get; set; } = default;

        public DateTime EndDate { get; set; } = default;
        
        public int Count { get; set; } = 0;

        public int Interval { get; set; } = 1;

        public WeekDays RecurringWeekDays { get; set; } = 0;

        public RecurringFrequency Frequency { get; set; } = 0;

        public bool IsParkingPlace { get; set; } = false;

        public string ParkingPlace { get; set; } = string.Empty;
        
        public bool IsOnlyFirstFree { get; set; } = false;

        public bool IsKitchenPresent { get; set; } = false;

        public bool IsMeetingRoomPresent { get; set; } = false;
        
        public bool IsNextToWindow { get; set; } = false;

        public bool IsVIP { get; set; } = false;

        public bool HasPC { get; set; } = false;

        public bool HasMonitor { get; set; } = false;

        public bool HasKeyboard { get; set; } = false;

        public bool HasMouse { get; set; } = false;

        public bool HasHeadset { get; set; } = false;

        public string NextStep { get; set; } = "Finish";

        public string TextMessage { get; set; } = "Not implemented yet";

        public List<string>? Propositions { get; set; } = new();

        public int CallbackMessageId { get; set; } = 0;

        public DateTime CalendarDate { get; set; } = default;

        public List<BookView> BookViews { get; set; } = new();

        public Guid BookingId { get; set; } = default;

        public bool IsOfficeReportSelected { get; set; } = false;

        public EditTypeEnum EditTypeEnum { get; set; } = EditTypeEnum.None;

        public Result GetResult()
        {
            return new Result() { TextMessage = TextMessage, Propositions = Propositions, IsSendMessage = CallbackMessageId == 0 };
        }

        public void SetByeAndFinish()
        {
            TextMessage = "Bye! See you later";
            Propositions = new();
            NextStep = "Finish";
        }

        public void SetResult(string textMessage = "Not implemented yet", List<string>? propositions = default, string nextStep = "Finish")
        {
            TextMessage = textMessage;
            Propositions = propositions;
            NextStep = nextStep;
        }

        public bool IsRecurring() => BookingType == BookingTypeEnum.Continuous || BookingType == BookingTypeEnum.Recurring;

        public AddRecurringBookingDto AddRecurringBookingDto() => new()
        {
            UserId = User.UserId,
            WorkplaceId = WorkplaceId,
            StartDate = StartDate,
            EndDate = EndDate,
            Count = Count,
            Interval = Interval,
            RecurringWeekDays = RecurringWeekDays,
            Frequency = Frequency,
            BookingType = BookingType,
            Summary = Summary()
        };

        public AddBookingDto AddBookingDto() => new()
        {
            UserId = User.UserId,
            WorkplaceId = WorkplaceId,
            Date = StartDate,
            BookingType = BookingType,
            Summary = Summary()
        };

        public bool IsBookingForOther(out string name)
        {
            name = string.Empty;
            var stream = User.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            if (jsonToken is JwtSecurityToken tokenS)
            {
                var tokenUserId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                if (Guid.TryParse(tokenUserId, out Guid realUserId))
                {
                    if (realUserId != User.UserId)
                    {
                        name += tokenS.Claims.First(c => c.Type == ClaimTypes.Surname).Value;
                        name += " " + tokenS.Claims.First(c => c.Type == ClaimTypes.GivenName).Value;
                        name += " (" + tokenS.Claims.First(c => c.Type == ClaimTypes.Role).Value + ")";
                        return true;
                    }
                }
            }
            return false;
        }

        public string Summary()
        {
            StringBuilder sb = new();
            sb.AppendLine(GetFullName());
            sb.AppendLine($"Email: {User.Email}");
            sb.AppendLine($"Office: <b>{OfficeName} {City}</b>");
            sb.AppendLine($"Floor number : {FloorName.Bold()}");
            sb.AppendLine($"Workplace : {WorkplaceName.Bold()}");
            sb.AppendLine($"Booking type: {BookingType.ToString().Bold()}");
            sb.Append(AddTextToCalendar(true));
            if (IsParkingPlace) 
            {
                sb.AppendLine($"Parking place: {ParkingPlace.Bold()}"); 
            }

            sb.AppendLine($"<i>booking creation time: {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")}</i>");
            if (IsBookingForOther(out string RealBookerName))
            {
                sb.AppendLine($"Booking made by: <b>{RealBookerName}</b>");
            }

            return sb.ToString();
        }


        public string GetFullName() 
        {
            return $"<b>{User.LastName} {User.FirstName}</b>"; 
        }

        public void InitRecurrencePattern()
        {
            StartDate = default;
            EndDate = default;
            Count = 0;
            Interval = 1;
            RecurringWeekDays = 0;
            Frequency = 0;
            CalendarDate = DateTime.Today;
        }

        public string AddTextToCalendar(bool isToSummary = false)
        {
            StringBuilder sb = new();
            switch (BookingType)
            {
                case BookingTypeEnum.OneDay:
                    {
                        if (StartDate == default)
                        {
                            sb.AppendLine("Select booking date!");
                        }
                        else
                        {
                            sb.AppendLine($"Booking date: {StartDate.ToString(Constants.DateFormat).Bold()}");
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        if (StartDate == default)
                        {
                            sb.AppendLine("Select the <b>start</b> date of the booking!");
                        }
                        else
                        {
                            sb.AppendLine($"Booking <b>start</b> date: {StartDate.ToString(Constants.DateFormat).Bold()}");
                            if (EndDate == default)
                            {
                                sb.AppendLine($"Select the <b>end</b> date of the booking!");
                            }
                            else
                            {
                                sb.AppendLine($"Booking <b>end</b> date: {EndDate.ToString(Constants.DateFormat).Bold()}");
                            }
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        if (StartDate == default)
                        {
                            sb.AppendLine("Select the <b>start</b> date of the booking!");
                        }
                        else
                        {
                            sb.AppendLine($"Booking <b>start</b> date: {StartDate.ToString(Constants.DateFormat).Bold()}");
                            if (EndDate == default)
                            {
                                if (Count == 0)
                                    sb.AppendLine($"Select the <b>end</b> date of the booking or increase occurrence!");
                            }
                            else
                            {
                                //sb.AppendLine($"Selected booking <b>end</b> date: {EndDate.ToString(Constants.DateFormat).Bold()}");
                            }

                            sb.AppendLine($"<b>Repeats</b> every {Interval.ToString().Bold()} {GetIntervalName().Bold()}");
                            if (Frequency == RecurringFrequency.Weekly)
                            {
                                sb.AppendLine($"Repeats on<b>:</b>{(RecurringWeekDays == WeekDays.None ? " <i>pick the days!</i>" : GetSelectedDaysOfWeek().Bold())}");
                            }

                            if (Count > 0)
                            {
                                sb.AppendLine($"Ends after {Count.ToString().Bold()} <b>occurrence</b>");
                                if (!isToSummary)
                                    sb.AppendLine($"<i>! to be able to select the <b>end</b> date, set the number of <b>occurrence</b> to zero</i>");
                            }
                            else
                            {
                                if (EndDate != default)
                                    sb.AppendLine($"Ending by date: {EndDate.ToString(Constants.DateFormat).Bold()}");
                            }
                        }
                        break;
                    }
            }

            return sb.ToString();
        }

        public string AddTextToCalendarForReport(bool isToSummary = false)
        {
            StringBuilder sb = new();
            if (StartDate == default)
            {
                sb.AppendLine("Select the <b>start</b> date for the report!");
            }
            else
            {
                sb.AppendLine($"Selected <b>start</b> date: {StartDate.ToString(Constants.DateFormat).Bold()}");
                if (EndDate == default)
                {
                    sb.AppendLine($"Select the <b>end</b> date for the report!");
                }
                else
                {
                    sb.AppendLine($"Selected <b>end</b> date: {EndDate.ToString(Constants.DateFormat).Bold()}");
                }
            }
            return sb.ToString();
        }

            private string GetIntervalName()
        {
            return Frequency switch
            {
                RecurringFrequency.Daily => Interval == 1 ? "day" : "days",
                RecurringFrequency.Weekly => Interval == 1 ? "week" : "weeks",
                RecurringFrequency.Monthly => Interval == 1 ? "month" : "months",
                RecurringFrequency.Yearly => Interval == 1 ? "year" : "years",
                _ => "NotImplemented"
            };
        }

        private string GetSelectedDaysOfWeek()
        {
            string str = string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Sunday) ? $" {WeekDays.Sunday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Monday) ? $" {WeekDays.Monday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Tuesday) ? $" {WeekDays.Tuesday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Wednesday) ? $" {WeekDays.Wednesday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Thursday) ? $" {WeekDays.Thursday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Friday) ? $" {WeekDays.Friday}," : string.Empty;
            str += RecurringWeekDays.HasFlag(WeekDays.Saturday) ? $" {WeekDays.Saturday}," : string.Empty;
            return str.TrimEnd(',');
        }
    }    
}
