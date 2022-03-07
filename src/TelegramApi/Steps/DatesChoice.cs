using Exadel.OfficeBooking.TelegramApi.StateMachine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.Steps
{
    public class DatesChoice : StateMachineStep
    {
        public override async Task<UserState> Execute(Update update)
        {
            string[] formatDates = { "dd.MM.yyyy","dd.M.yyyy", "d.MM.yyyy", "d.M.yyyy" };
            string? text = update.Message?.Text;
            string start = "Start";

            switch (_state.BookingType)
            {
                case BookingTypeEnum.None:
                    {
                        if (_state.Propositions == null)
                        {
                            return _state;
                        }

                        // One day
                        if (text == _state.Propositions[0])
                        {
                            _state.BookingType = BookingTypeEnum.OneDay;
                            start = string.Empty;
                        }
                        // Continuous
                        else if (text == _state.Propositions[1])
                        {
                            _state.BookingType = BookingTypeEnum.Continuous;
                        }
                        // Reccuring
                        else if (text == _state.Propositions[2])
                        {
                            _state.BookingType = BookingTypeEnum.Recurring;
                        }
                        _state.TextMessage = $"Enter the {start} date in the format dd.mm.yyyy";
                        _state.Propositions = default;
                        // state.NextState does not change
                        break;
                    }
                case BookingTypeEnum.OneDay:
                    {
                        if (DateTime.TryParseExact(text, formatDates, CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out DateTime dateStart))
                        {
                            _state.DateStart = dateStart;
                            _state.TextMessage = "Would you like to add parking place?";
                            _state.Propositions = new() { "yes", "no" };
                            _state.NextStep = nameof(ParkingChoice);
                        }
                        else
                        {
                            _state.TextMessage = "Invalid format, try again";
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        //startDate is being selected
                        if (_state.DateStart == default)
                        {
                            if (DateTime.TryParseExact(text, formatDates, CultureInfo.InvariantCulture, 
                                    DateTimeStyles.None, out DateTime dateStart))
                            {
                                _state.DateStart = dateStart;
                                _state.TextMessage = "Enter the End date in the format dd.mm.yyyy";
                            }
                            else _state.TextMessage = "Invalid format, try again";
                        }
                        //state when startDate was already selected
                        else if (_state.DateStart != default)
                        {
                            if (DateTime.TryParseExact(text, formatDates,
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateEnd))
                            {
                                _state.DateEnd = dateEnd;
                                _state.TextMessage = "Would you like to add parking place?";
                                _state.Propositions = new() {"yes", "no"};
                                _state.NextStep = nameof(ParkingChoice);
                            }
                            else _state.TextMessage = "Invalid format, try again";
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        //order of these if statements matter!!!
                        if (_state.DateStart == default)
                        {
                            if (DateTime.TryParseExact(text, formatDates, CultureInfo.InvariantCulture, 
                                    DateTimeStyles.None, out DateTime dateStart))
                            {
                                _state.DateStart = dateStart;
                                
                                //send buttons to user to specify type of frequency
                                List<string> frequencyTypes = new List<string>{"Daily" , "Weekly", "Monthly", "Yearly"};
                                _state.TextMessage = "Choose frequency type";
                                _state.Propositions = frequencyTypes;
                            }
                            else _state.TextMessage = "Invalid format, try again";
                        }
                        //startDate is selected, now Frequency is being selected
                        else if (_state.Frequency == null)
                        {
                            if (_state.Propositions == null) return _state;
                            //daily
                            if (text == _state.Propositions[0]) _state.Frequency = RecurringFrequency.Daily;
                            //weekly
                            else if (text == _state.Propositions[1])
                            {
                                _state.Frequency = RecurringFrequency.Weekly;
                                _state.IsRecurringFrequencyWeekly = true;
                                _state.TextMessage = "Choose weekdays\n" +
                                                     "Example: Monday,Tuesday,Sunday";
                                _state.Propositions = new List<string>();
                                break;
                            }
                            //Monthly
                            else if (text == _state.Propositions[2]) _state.Frequency = RecurringFrequency.Monthly;
                            //Yearly
                            else if(text == _state.Propositions[3]) _state.Frequency = RecurringFrequency.Yearly;
                            
                            //now ask whether they want to choose EndDate
                            _state.TextMessage = "Do you want to specify end date of your booking?";
                            _state.Propositions = new List<string> {"yes", "no"};
                        }
                        //Frequency was selected as Weekly
                        else if (_state.RecurringWeekDays == null && _state.IsRecurringFrequencyWeekly != null 
                                                                  && (bool) _state.IsRecurringFrequencyWeekly)
                        {
                            int weekdays = 0;
                            string[] userChosenWeekDays = text.Split(',');
                            foreach (var day in userChosenWeekDays)
                            {
                                string dayLower = day.ToLower();
                                if (dayLower == "sunday") weekdays += (int) WeekDays.Sunday;
                                if (dayLower == "monday") weekdays += (int) WeekDays.Monday;
                                if (dayLower == "tuesday") weekdays += (int) WeekDays.Tuesday;
                                if (dayLower == "wednesday") weekdays += (int) WeekDays.Wednesday;
                                if (dayLower == "thursday") weekdays += (int) WeekDays.Thursday;
                                if (dayLower == "friday") weekdays += (int) WeekDays.Friday;
                                if (dayLower == "saturday") weekdays += (int) WeekDays.Saturday;
                            }
                            _state.RecurringWeekDays = (WeekDays) weekdays;
                            
                            //now ask whether they want to choose EndDate
                            _state.TextMessage = "Do you want to specify end date of your booking?";
                            _state.Propositions = new List<string> {"yes", "no"};
                        }
                        //IsEndDateGiven is not initialized yet, this if statement will init it here
                        else if (_state.IsEndDateGiven == null)
                        {
                            if (_state.Propositions == null) return _state;
                            //yes
                            if (text == _state.Propositions[0])
                            {
                                _state.IsEndDateGiven = true;
                                _state.TextMessage = "Enter the End date in the format dd.mm.yyyy";
                                _state.Propositions = new List<string>();
                            }
                            //no
                            else if (text == _state.Propositions[1])
                            {
                                _state.IsEndDateGiven = false;
                                _state.TextMessage = "Would you like to specify count?";
                                _state.Propositions = new List<string> {"yes", "no"};
                            }
                        }
                        else if (_state.DateEnd == default && (bool) _state.IsEndDateGiven)
                        {
                            if (DateTime.TryParseExact(text, formatDates,
                                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateEnd))
                            {
                                _state.DateEnd = dateEnd;
                                _state.TextMessage = "Would you like to specify count?";
                                _state.Propositions = new List<string> {"yes", "no"};
                            }
                            else _state.TextMessage = "Invalid format, try again";
                        }
                        else if (_state.IsCountGiven == null)
                        {
                            if (_state.Propositions == null) return _state;
                            //yes
                            if (text == _state.Propositions[0])
                            {
                                _state.IsCountGiven = true;
                                _state.TextMessage = "Enter the count";
                            }

                            if (text == _state.Propositions[1])
                            {
                                _state.IsCountGiven = false;
                                _state.TextMessage = "Would you like to enter interval";
                                _state.Propositions = new List<string> {"yes", "no"};
                            }
                        }
                        else if (_state.Count == null && (bool) _state.IsCountGiven)
                        {
                            if (int.TryParse(text, out int count))
                            {
                                _state.Count = count;
                                _state.TextMessage = "Would you like to enter interval";
                                _state.Propositions = new List<string> {"yes", "no"};
                            }
                            else _state.TextMessage = "Enter valid number";
                        }
                        else if (_state.IsIntervalGiven == null)
                        {
                            if (_state.Propositions == null) return _state;
                            //yes
                            if (text == _state.Propositions[0])
                            {
                                _state.IsIntervalGiven = true;
                                _state.TextMessage = "Enter the interval";
                                _state.Propositions = new List<string>();
                            }
                            //no
                            else if (text == _state.Propositions[1])
                            {
                                _state.IsIntervalGiven = false;
                                _state.TextMessage = "Would you like to add parking place?";
                                _state.Propositions = new() {"yes", "no"};
                                _state.NextStep = nameof(ParkingChoice);
                            }
                        }
                        else if (_state.Interval == null && (bool) _state.IsIntervalGiven)
                        {
                            if (int.TryParse(text, out int interval))
                            {
                                _state.Interval = interval;
                                _state.TextMessage = "Would you like to add parking place?";
                                _state.Propositions = new() {"yes", "no"};
                                _state.NextStep = nameof(ParkingChoice);
                            }
                            else _state.TextMessage = "Enter valid number";
                        }
                        break;
                    }
            }
            return _state;
        }
    }
}