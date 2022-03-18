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
        public static async Task<int> SendBookingList(this TelegramBotClient bot, Update update, string text, Dictionary<string, string> dictionary)
        {
            _ = await Send(text);
            List<int> listOfIdMessages = new();
            for (int i = 0; i < dictionary.Count; i++)
            {
                string textBook = dictionary.ElementAt(i).Value;
                textBook = string.Join("\r\n", textBook.Split("\r\n")[2..]);
                Message message = await Send($"{(i + 1).ToString().Bold()}.\r\n" + textBook, CreateEditRow());
                listOfIdMessages.Add(message.MessageId);
            }

            Message naviMessage = await Send("Check the boxes to cancel", CreateBackAndCancelAll());
            return naviMessage.MessageId;

            async Task<Message> Send(string textMessage, InlineKeyboardMarkup? inlineKeyboard = null)
            {
                return await bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                                      text: textMessage,
                                                      parseMode: ParseMode.Html,
                                                      replyMarkup: inlineKeyboard);
            }
        }

        public static async Task<int> SendCalendar(this TelegramBotClient bot, Update update, DateTime date, string text, RecurrencePattern recurrPatern)
        {
            var calendarMarkup = Markup.Calendar(date, recurrPatern, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
            Message sendMess = await bot.SendTextMessageAsync(
                                                        chatId: update.Message.Chat.Id,
                                                        text: text,
                                                        parseMode: ParseMode.Html,
                                                        replyMarkup: calendarMarkup);
            return sendMess.MessageId;
        }

        public static async Task EditCalendar(this TelegramBotClient bot, Update update, DateTime date, string text, RecurrencePattern recurrPatern)
        {
            try
            {
                var calendarMarkup = Markup.Calendar(date, recurrPatern, CultureInfo.GetCultureInfo("en-US").DateTimeFormat);
                await bot.EditMessageTextAsync(
                                                    chatId: update.CallbackQuery.Message.Chat.Id,
                                                    messageId: update.CallbackQuery.Message.MessageId,
                                                    text: text,
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

        public static async Task<int> DeleteInlineKeyboardWithText(this TelegramBotClient bot, Update update)
        {
            await bot.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, text: "-");
            return default;
        }

        private static InlineKeyboardMarkup CreateInlineKeyboardMarkup(Dictionary<string, string> dictionary)
        {
            InlineKeyboardButton[][] inlineKeyboardButtons = dictionary
                .Select(d => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(d.Value, d.Key )}).ToArray();
            return new InlineKeyboardMarkup(inlineKeyboardButtons);
        }

        private static InlineKeyboardMarkup CreateEditRow()
        {
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
            {
                new InlineKeyboardButton[]
                {
                    "Change",
                    "Cancel",
                    "◻️"
                }
            });
        }

        private static InlineKeyboardMarkup CreateBackAndCancelAll()
        {
            return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("<< Back", "Back:true"),
                    InlineKeyboardButton.WithCallbackData("CheckAll", "CheckAll:true"),
                    InlineKeyboardButton.WithCallbackData("Cancel all checked", "cancelAll:true")
                }
            });
        }
    }
}
