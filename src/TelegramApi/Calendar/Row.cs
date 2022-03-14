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

        public static IEnumerable<InlineKeyboardButton> DayOfWeek(DateTimeFormatInfo dtfi)
        {
            var dayNames = new InlineKeyboardButton[7];
            var firstDayOfWeek = (int)dtfi.FirstDayOfWeek;
            for (int i = 0; i < 7; i++)
            {
                dayNames[i] = dtfi.AbbreviatedDayNames[(firstDayOfWeek + i) % 7];
            }
            return dayNames;
        }

        public static IEnumerable<IEnumerable<InlineKeyboardButton>> Month(DateTime date, DateTimeFormatInfo dtfi)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;

            for (int dayOfMonth = 1, weekNum = 0; dayOfMonth <= lastDayOfMonth; weekNum++)
            {
                yield return NewWeek(weekNum, ref dayOfMonth, ref firstDayOfMonth);
            }

            IEnumerable<InlineKeyboardButton> NewWeek(int weekNum, ref int dayOfMonth, ref DateTime date)
            {
                var week = new InlineKeyboardButton[7];

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    if ((weekNum == 0 && dayOfWeek < FirstDayOfWeek())
                       ||
                       dayOfMonth > lastDayOfMonth
                    )
                    {
                        week[dayOfWeek] = " ";
                        continue;
                    }

                    bool isImposDate = date < DateTime.Today || date > DateTime.Today.AddMonths(3);
                    bool isDateToday = DateTime.Today == date;
                    bool isMaxDate = DateTime.Today.AddMonths(3) == date;

                    string dateVal = dayOfMonth.ToString();
                    string dateKey = $"{Constants.PickDate}{date.ToString(Constants.DateFormat)}";

                    dateVal = isDateToday ? "[" + dateVal + "]" : dateVal;
                    //dateVal = isMaxDate ? dateVal + "<" : dateVal;
                    dateVal = isImposDate ? " " : dateVal;
                    dateKey = isImposDate ? "impossible date" : dateKey;
                    
                    week[dayOfWeek] = InlineKeyboardButton.WithCallbackData(dateVal, dateKey);

                    dayOfMonth++;
                    date = date.AddDays(1);
                }
                return week;

                int FirstDayOfWeek() =>
                    (7 + (int)firstDayOfMonth.DayOfWeek - (int)dtfi.FirstDayOfWeek) % 7;
            }
        }

        public static IEnumerable<InlineKeyboardButton> Controls(in DateTime date) =>
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(
                    date.AddMonths(-1) < DateTime.Today ? " " : "<",
                    date.AddMonths(-1) < DateTime.Today ? " " : $"{Constants.ChangeTo}{date.AddMonths(-1).ToString(Constants.DateFormat)}"
                ),
                " ",
                InlineKeyboardButton.WithCallbackData(
                    date.AddMonths(1) > DateTime.Today.AddMonths(3) ? " " : ">",
                    date.AddMonths(1) > DateTime.Today.AddMonths(3) ? " " : $"{Constants.ChangeTo}{date.AddMonths(1).ToString(Constants.DateFormat)}"
                ),
            };

        public static IEnumerable<InlineKeyboardButton> Close() =>
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("[ Close ]", Constants.Close + "true")
            };
    }
}
