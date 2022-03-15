using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi
{
    public static class TelegramBotClientExtensions
    {
        public static async Task<int> SendCalendar(this TelegramBotClient bot, Update update, DateTime date, RecurrencePattern recurrPatern, BookingTypeEnum bookingType)
        {
            var calendarMarkup = Markup.Calendar(date, recurrPatern, bookingType, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
            Message sendMess = await bot.SendTextMessageAsync(
                                                        chatId: update.Message.Chat.Id,
                                                        text: AddTextToCalendar(recurrPatern, bookingType),
                                                        parseMode: ParseMode.Html,
                                                        replyMarkup: calendarMarkup);
            return sendMess.MessageId;
        }

        public static async Task EditCalendar(this TelegramBotClient bot, Update update, DateTime date, RecurrencePattern recurrPatern, BookingTypeEnum bookingType)
        {
            try
            {
                var calendarMarkup = Markup.Calendar(date, recurrPatern, bookingType, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                await bot.EditMessageTextAsync(
                                                    chatId: update.CallbackQuery.Message.Chat.Id,
                                                    messageId: update.CallbackQuery.Message.MessageId,
                                                    text: AddTextToCalendar(recurrPatern, bookingType),
                                                    parseMode: ParseMode.Html,
                                                    replyMarkup: calendarMarkup);

            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                Console.WriteLine(ex.Message);
                await bot.AnswerCallbackQueryAsync(
                            callbackQueryId: update.CallbackQuery.Id,
                            text: $"You click too fast. Slow down please!");
            }
        }

        public static async Task EchoCallbackQuery(this TelegramBotClient bot, Update update)
        {
            await bot.AnswerCallbackQueryAsync(
                            callbackQueryId: update.CallbackQuery.Id,
                            text: update.CallbackQuery.Data);
        }

        public static async Task<int> SendInlineKbList(this TelegramBotClient bot, Update update, string text, Dictionary<string, string> dictionary)
        {
            Message sendMess = await bot.SendTextMessageAsync(
                                                        chatId: update.Message.Chat.Id,
                                                        text: text,
                                                        replyMarkup: CreateInlineKeyboardMarkup(dictionary));
            return sendMess.MessageId;
        }

        public static async Task EditInlineKbList(this TelegramBotClient bot, Update update, Dictionary<string, string> dictionary)
        {
            try
            {
                await bot.EditMessageReplyMarkupAsync(
                                                    chatId: update.CallbackQuery.Message.Chat.Id,
                                                    messageId: update.CallbackQuery.Message.MessageId,
                                                    replyMarkup: CreateInlineKeyboardMarkup(dictionary));
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                await bot.AnswerCallbackQueryAsync(
                            callbackQueryId: update.CallbackQuery.Id,
                            text: $"You click too fast. Slow down please!");
                Console.WriteLine("You click too fast. Slow down please!" + ex.Message);
            }
        }

        public static async Task<int> DeleteInlineKeyboard(this TelegramBotClient bot, Update update)
        {
            await bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
            return default;
        }

        private static InlineKeyboardMarkup CreateInlineKeyboardMarkup(Dictionary<string, string> dictionary)
        {
            InlineKeyboardButton[][] inlineKeyboardButtons = dictionary
                .Select(d => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(d.Value, d.Key )}).ToArray();
            return new InlineKeyboardMarkup(inlineKeyboardButtons);
        }

        private static string AddTextToCalendar(RecurrencePattern rp, BookingTypeEnum bookingType)
        {
            StringBuilder sb = new();
            switch (bookingType)
            {
                case BookingTypeEnum.OneDay:
                    {
                        if (rp.StartDate == default)
                        {
                            sb.AppendLine("Select booking date!");
                        }
                        else
                        {
                            sb.AppendLine($"Selected booking date: {rp.StartDate.ToString(Constants.DateFormat).Bold()}");
                        }
                        break;
                    }
                case BookingTypeEnum.Continuous:
                    {
                        if (rp.StartDate == default)
                        {
                            sb.AppendLine("Select booking <b>start</b> date!");
                        }
                        else
                        {
                            sb.AppendLine($"Selected <b>start</b> date: {rp.StartDate.ToString(Constants.DateFormat).Bold()}");
                            if (rp.EndDate == default)
                            {
                                sb.AppendLine($"Select <b>end</b> date!");
                            }
                            else
                            {
                                sb.AppendLine($"Selected <b>end</b> date: {rp.EndDate.ToString(Constants.DateFormat).Bold()}");
                            }
                        }
                        break;
                    }
                case BookingTypeEnum.Recurring:
                    {
                        if (rp.StartDate == default)
                        {
                            sb.AppendLine("Select booking <b>start</b> date!");
                        }
                        else
                        {
                            sb.AppendLine($"Selected <b>start</b> date: {rp.StartDate.ToString(Constants.DateFormat).Bold()}");
                            if (rp.EndDate == default)
                            {
                                if (rp.Count == 0)
                                sb.AppendLine($"Select <b>end</b> date or increase the count of repetitions!");
                            }
                            else
                            {
                                sb.AppendLine($"Selected <b>end</b> date: {rp.EndDate.ToString(Constants.DateFormat).Bold()}");
                            }

                            sb.AppendLine($"Recurring frequency: \"{rp.Frequency.ToString().Bold()}\" with interval: {rp.Interval.ToString().Bold()}");
                            if (rp.Count > 0)
                            {
                                sb.AppendLine($"Ending after {rp.Count.ToString().Bold()} repetitions");
                                sb.AppendLine($"<i>! to be able to select the <b>end</b> date, set the Count of repetitions to zero</i>");
                            }
                            else
                            {
                                if (rp.EndDate != default)
                                    sb.AppendLine($"Ending by date: {rp.EndDate.ToString(Constants.DateFormat).Bold()}");
                            }
                        }
                        break;
                    }
            }

            return sb.ToString();
        }
    }
}
