using Exadel.OfficeBooking.TelegramApi.Calendar;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.BookingDto;
using Exadel.OfficeBooking.TelegramApi.Tgm;
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
        public static async Task<BookViewResponse> SendBookingList(this TelegramBotClient bot, Update update, string text, Dictionary<string, string> dictionary)
        {
            _ = await Send(text);
            List<BookView> bookViews = new();
            for (int i = 0; i < dictionary.Count; i++)
            {
                string textBook = dictionary.ElementAt(i).Value;
                string bookingId = dictionary.ElementAt(i).Key;
                textBook = string.Join("\r\n", textBook.Split("\r\n")[2..]);
                Message message = await Send($"{(i + 1).ToString().Bold()}.\r\n" + textBook, InlineKbMarkups.CreateEditRow(bookingId, false));
                bookViews.Add(new BookView { MessageId = message.MessageId, BookingId = bookingId, IsChecked = false});
            }

            Message backMessage = await Send("Check the boxes to cancel", InlineKbMarkups.CreateBackAndCancelAll(false));
            return new BookViewResponse { BookViews = bookViews, BackMessageId = backMessage.MessageId };

            async Task<Message> Send(string textMessage, InlineKeyboardMarkup? inlineKeyboard = null)
            {
                return await bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                                      text: textMessage,
                                                      parseMode: ParseMode.Html,
                                                      replyMarkup: inlineKeyboard);
            }
        }

        public static async Task UpdateBookinList(this TelegramBotClient bot, Update update, List<BookView> bookViews)
        {
            foreach (var bookView in bookViews)
            {
                await bot.EditMessageReplyMarkupAsync(chatId: update.CallbackQuery.Message.Chat.Id,
                                                      messageId: bookView.MessageId,
                                                      replyMarkup: InlineKbMarkups.CreateEditRow(bookView.BookingId, bookView.IsChecked));
            }
        }

        public static async Task UpdateBackAndCanselAllBookinList(this TelegramBotClient bot, Update update, int messageId, bool isAllChecked)
        {
                await bot.EditMessageReplyMarkupAsync(chatId: update.CallbackQuery.Message.Chat.Id,
                                                      messageId: messageId,
                                                      replyMarkup: InlineKbMarkups.CreateBackAndCancelAll(isAllChecked));
        }

        public static async Task<int> DeleteBookinList(this TelegramBotClient bot, Update update, List<int> identifires, int lastMessage = 0, bool isOnlyButtons = false)
        {
            foreach(var id in identifires)
            {
                if (isOnlyButtons)
                    await bot.EditMessageReplyMarkupAsync(chatId: update.CallbackQuery.Message.Chat.Id, messageId: id, replyMarkup: null);
                else
                    await bot.DeleteMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, messageId: id);
            }

            if(lastMessage != 0)
                await bot.DeleteMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, messageId: lastMessage);
            return 0;
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
            await bot.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
            return default;
        }

        private static InlineKeyboardMarkup CreateInlineKeyboardMarkup(Dictionary<string, string> dictionary)
        {
            InlineKeyboardButton[][] inlineKeyboardButtons = dictionary
                .Select(d => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(d.Value, d.Key )}).ToArray();
            return new InlineKeyboardMarkup(inlineKeyboardButtons);
        }
    }
}
