using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.Calendar
{
    public static class Row
    {
        public static IEnumerable<InlineKeyboardButton> Date(in DateTime date, DateTimeFormatInfo dtfi) =>
            new InlineKeyboardButton[]
            {
            InlineKeyboardButton.WithCallbackData($"» {date.ToString("Y", dtfi)} «"," ")
            };

        public static IEnumerable<InlineKeyboardButton> DayOfWeek(int weekDays, RecurringFrequency frequency, DateTimeFormatInfo dtfi)
        {
            //Console.WriteLine("weekDays = " + weekDays);
            var dayNames = new InlineKeyboardButton[7];
            var firstDayOfWeek = (int)dtfi.FirstDayOfWeek;
            if (frequency == RecurringFrequency.Weekly)
            {
                for (int i = 0; i < 7; i++)
                {
                    string dayKey;
                    int dayOfWeek = 1 << i;
                    string dayVal = dtfi.AbbreviatedDayNames[(firstDayOfWeek + i) % 7];
                    if ((weekDays & (dayOfWeek)) != 0)
                    {
                        dayVal = dayVal[..1] + "✔";
                        dayKey = Constants.DayOfWeek + (weekDays & (~dayOfWeek));
                        //Console.WriteLine($"i = {i+1}, {~dayOfWeek}");
                    }
                    else
                    {
                        dayKey = Constants.DayOfWeek + (weekDays | dayOfWeek);
                    }
                    dayNames[i] = InlineKeyboardButton.WithCallbackData(dayVal, dayKey);
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    dayNames[i] = dtfi.AbbreviatedDayNames[(firstDayOfWeek + i) % 7];
                }
            }

            return dayNames;
        }

        public static IEnumerable<IEnumerable<InlineKeyboardButton>> Month(DateTime date, List<DateTime>? hDates, DateTimeFormatInfo dtfi)
        {
            var listOfWeek = new List<IEnumerable<InlineKeyboardButton>>();
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;
            int numOfEmptyDays = 0;

            for (int dayOfMonth = 1, weekNum = 0; dayOfMonth <= lastDayOfMonth; weekNum++)
            {
                var newWeek = NewWeek(weekNum, ref dayOfMonth, ref firstDayOfMonth, ref numOfEmptyDays);
                if (numOfEmptyDays != 7)
                {
                    listOfWeek.Add(newWeek);
                }

                numOfEmptyDays = 0;
            }

            IEnumerable<InlineKeyboardButton> NewWeek(int weekNum, ref int dayOfMonth, ref DateTime date, ref int numOfEmptyDays)
            {
                var week = new InlineKeyboardButton[7];

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    if ((weekNum == 0 && dayOfWeek < FirstDayOfWeek()) || dayOfMonth > lastDayOfMonth)
                    {
                        week[dayOfWeek] = " ";
                        numOfEmptyDays += 1;
                        continue;
                    }

                    bool isImposDate = date < DateTime.Today || date > DateTime.Today.AddMonths(3);
                    bool isDateToday = DateTime.Today == date;

                    string dateVal = dayOfMonth.ToString();
                    string dateKey = $"{Constants.PickDate}{date.ToString(Constants.DateFormat)}";

                    dateVal = isDateToday ? "[" + dateVal + "]" : dateVal;
                    if (isImposDate)
                    {
                        week[dayOfWeek] = " ";
                        numOfEmptyDays += 1;
                    }
                    else
                    {
                        if (hDates != null && hDates.Contains(date))
                        {
                            dateVal += "*";
                        }

                        week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(dateVal, dateKey);
                    }

                    dayOfMonth++;
                    date = date.AddDays(1);
                }

                return week;
                int FirstDayOfWeek() => (7 + (int)firstDayOfMonth.DayOfWeek - (int)dtfi.FirstDayOfWeek) % 7;
            }
            return listOfWeek;
        }

        public static IEnumerable<InlineKeyboardButton> Controls(in DateTime date) =>
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(
                    date.AddMonths(-1) < DateTime.Today ? " " : "<",
                    date.AddMonths(-1) < DateTime.Today ? " " : $"{Constants.ChangeTo}{date.AddMonths(-1).ToString(Constants.DateFormat)}"
                ),
                InlineKeyboardButton.WithCallbackData("[ Ok ]", Constants.Close + "true"),
                InlineKeyboardButton.WithCallbackData(
                    date.AddMonths(1) > DateTime.Today.AddMonths(3) ? " " : ">",
                    date.AddMonths(1) > DateTime.Today.AddMonths(3) ? " " : $"{Constants.ChangeTo}{date.AddMonths(1).ToString(Constants.DateFormat)}"
                ),
            };

        public static IEnumerable<InlineKeyboardButton> Freq(RecurringFrequency recurringFrequency)
        {
            string dailyVal = RecurringFrequency.Daily.ToString();
            string dailyKey = Constants.Frequency + (int)RecurringFrequency.Daily;
            string weeklyVal = RecurringFrequency.Weekly.ToString();
            string weeklyKey = Constants.Frequency + (int)RecurringFrequency.Weekly;
            string monthlyVal = RecurringFrequency.Monthly.ToString();
            string monthlyKey = Constants.Frequency + (int)RecurringFrequency.Monthly;

            if (recurringFrequency == RecurringFrequency.Daily)
            {
                dailyVal += "✔";
                dailyKey = " ";
            }

            if (recurringFrequency == RecurringFrequency.Weekly)
            {
                weeklyVal += "✔";
                weeklyKey = " ";
            }

            if (recurringFrequency == RecurringFrequency.Monthly)
            {
                monthlyVal += "✔";
                monthlyKey = " ";
            }

            return new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(dailyVal, dailyKey),
                InlineKeyboardButton.WithCallbackData(weeklyVal,weeklyKey),
                InlineKeyboardButton.WithCallbackData(monthlyVal, monthlyKey)
            };
        }

        public static IEnumerable<InlineKeyboardButton> RecurrControls(int count, int interval) =>
            new InlineKeyboardButton[]
            {
                interval == 1 ? " " : InlineKeyboardButton.WithCallbackData("Interval-", Constants.Interval + "-1"),
                InlineKeyboardButton.WithCallbackData("Interval+", Constants.Interval + "1"),
                count == 0 ? " " : InlineKeyboardButton.WithCallbackData("Count-", Constants.Count + "-1"),
                InlineKeyboardButton.WithCallbackData("Count+", Constants.Count + "1")
            };
    }
}
