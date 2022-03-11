using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi
{
    public static class TelegramBotClientExtensions
    {
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
            await bot.EditMessageReplyMarkupAsync(
                                                chatId: update.CallbackQuery.Message.Chat.Id,
                                                messageId: update.CallbackQuery.Message.MessageId,
                                                replyMarkup:CreateInlineKeyboardMarkup(dictionary));
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
    }
}
