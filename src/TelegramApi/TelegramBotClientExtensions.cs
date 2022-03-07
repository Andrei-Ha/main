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
            InlineKeyboardButton[][] inlineKeyboardButtons = dictionary
                .Select(d => new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(d.Key, d.Value )}).ToArray();
            InlineKeyboardMarkup inlineKeyboard = new(inlineKeyboardButtons);

            Message sendMess = await bot.SendTextMessageAsync(
                                                        chatId: update.Message.Chat.Id,
                                                        text: text,
                                                        replyMarkup: inlineKeyboard);
            return sendMess.MessageId;
        }

        public static async Task DeleteInlineKeyboard(this TelegramBotClient bot, Update update)
        {
            await bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
        }
    }
}
