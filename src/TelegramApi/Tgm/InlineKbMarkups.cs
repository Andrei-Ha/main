using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.Tgm
{
    public static class InlineKbMarkups
    {
        public static InlineKeyboardMarkup CreateEditRow(string bookingId, bool isChecked)
        {
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Change",$"Change:{bookingId}"),
                    InlineKeyboardButton.WithCallbackData("Cancel", $"Cancel:{bookingId}"),
                    InlineKeyboardButton.WithCallbackData(isChecked ? "☑" : "◻️",isChecked ? $"Check:{bookingId}/false" : $"Check:{bookingId}/true")
                }
            });
        }

        public static InlineKeyboardMarkup CreateBackAndCancelAll(bool isAllChecked)
        {
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Cancel all checked", "CancelChecked:true"),
                    InlineKeyboardButton.WithCallbackData(isAllChecked ? "UncheckAll ☑" : "CheckAll ◻️", isAllChecked ? "CheckAll:false" : "CheckAll:true")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("<< Back", "Back:true")
                }
            });
        }
    }
}
