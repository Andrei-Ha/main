using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.Calendar
{
    public static class Markup
    {
        public static InlineKeyboardMarkup Calendar(in DateTime date, RecurrencePattern rp, DateTimeFormatInfo dtfi)
        {
            List<DateTime> hDites = new();
            Console.WriteLine("count = " + rp.Count);
            switch (rp.BookingType)
            {
                case BookingTypeEnum.OneDay:
                    {
                        hDites.Add(rp.StartDate);
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        var startDate = rp.StartDate;
                        var endDate = rp.EndDate == default ? startDate : rp.EndDate;
                        while ( startDate <= endDate)
                        {
                            hDites.Add(startDate);
                            startDate = startDate.AddDays(1);
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        hDites = GetRecurringBookingDates(rp);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var keyboardRows = new List<IEnumerable<InlineKeyboardButton>>();
            if (rp.BookingType == BookingTypeEnum.Recurring)
            {
                keyboardRows.Add(Row.Freq(rp.Frequency));
                keyboardRows.Add(Row.RecurrControls(rp.Count, rp.Interval));
            }

            keyboardRows.Add(Row.Date(date, dtfi));
            keyboardRows.Add(Row.DayOfWeek((int)rp.RecurringWeekDays, rp.Frequency, dtfi));
            keyboardRows.AddRange(Row.Month(date, hDites, dtfi));
            keyboardRows.Add(Row.Controls(date, rp));
            keyboardRows.Add(Row.Back());

            return new InlineKeyboardMarkup(keyboardRows);
        }

        public static List<DateTime> GetRecurringBookingDates(RecurrencePattern booking)
        {
            List<DateTime> recurringDates = new();

            if (booking.Interval < 1) booking.Interval = 1;

            if (booking.EndDate != null)
            {
                var curDate = booking.StartDate;
                while (curDate <= booking.EndDate)
                {
                    if (booking.Frequency == RecurringFrequency.Daily)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddDays(booking.Interval);
                    }

                    if (booking.Frequency == RecurringFrequency.Weekly)
                    {
                        //add date to recurringDates
                        if (booking.RecurringWeekDays.HasFlag(GetDayOfWeek(curDate)))
                            recurringDates.Add(curDate);

                        //go to next day of week
                        curDate = curDate.AddDays(1);

                        //this is for interval to skip weeks if necessary
                        if (curDate.DayOfWeek == DayOfWeek.Sunday && booking.Interval > 1)
                            curDate = curDate.AddDays(7 * (booking.Interval - 1));
                    }

                    if (booking.Frequency == RecurringFrequency.Monthly)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddMonths(booking.Interval);
                    }

                    if (booking.Frequency == RecurringFrequency.Yearly)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddYears(booking.Interval);
                    }
                }
            }

            if (booking.Count != null)
            {
                var curDate = booking.StartDate;
                var initDayOfWeek = (int)curDate.DayOfWeek;

                //when count is weekly we have to add count*7 days per one count
                var countTimes = 1;
                if (booking.Frequency == RecurringFrequency.Weekly)
                    countTimes = 7;

                for (var i = 0; i < booking.Count * countTimes; i++)
                {
                    if (booking.Frequency == RecurringFrequency.Daily)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddDays(booking.Interval);
                    }

                    if (booking.Frequency == RecurringFrequency.Weekly)
                    {
                        //add date to recurringDates
                        if (booking.RecurringWeekDays.HasFlag(GetDayOfWeek(curDate)))
                            recurringDates.Add(curDate);

                        //go to next day of week
                        curDate = curDate.AddDays(1);

                        if (curDate.DayOfWeek == DayOfWeek.Monday && booking.Interval > 1)
                            curDate = curDate.AddDays(7 * (booking.Interval - 1));
                    }

                    if (booking.Frequency == RecurringFrequency.Monthly)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddMonths(booking.Interval);
                    }

                    if (booking.Frequency == RecurringFrequency.Yearly)
                    {
                        recurringDates.Add(curDate);
                        curDate = curDate.AddYears(booking.Interval);
                    }
                }
            }

            return recurringDates;
        }

        private static WeekDays GetDayOfWeek(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday) return WeekDays.Sunday;
            if (date.DayOfWeek == DayOfWeek.Monday) return WeekDays.Monday;
            if (date.DayOfWeek == DayOfWeek.Tuesday) return WeekDays.Tuesday;
            if (date.DayOfWeek == DayOfWeek.Wednesday) return WeekDays.Wednesday;
            if (date.DayOfWeek == DayOfWeek.Thursday) return WeekDays.Thursday;
            if (date.DayOfWeek == DayOfWeek.Friday) return WeekDays.Friday;

            return WeekDays.Saturday;
        }
    }
}
