using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotClient _bot;
        private readonly StateMachine.StateMachine _fsm;

        public TelegramBotController(TelegramBot telegramBot, StateMachine.StateMachine stateMachine )
        {
            _bot = telegramBot.GetBot().Result;
            _fsm = stateMachine;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello Team3!");
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update?.Type != UpdateType.Message && update?.Type != UpdateType.CallbackQuery)
                return Ok();
            
            var message = update.Message!;
            if (update?.Type == UpdateType.Message && message.Type != MessageType.Text)
                return Ok();

            long chatId;
            if (update?.Type == UpdateType.CallbackQuery)
            {
                chatId = update.CallbackQuery.Message.Chat.Id;
            }
            else
            {
                chatId = update.Message.Chat.Id;
            }
            Console.WriteLine(chatId);

            // ! This method tell the user that something is happening on the bot's side !
            await _bot.SendChatActionAsync(chatId, ChatAction.Typing);
            await _fsm.GetState(chatId);
            var result = await _fsm.Process(update);


            // Create custom keyboard or Remove
            IReplyMarkup replyMarkup;
            if (result.Propositions == null || result.Propositions.Count == 0)
            {
                replyMarkup = new ReplyKeyboardRemove();
            }
            else
            {
                KeyboardButton[][] keyboardButtons = result.Propositions
                    .Select(k => new KeyboardButton[] { k }).ToArray();
                ReplyKeyboardMarkup replyKeyboardMarkup = new(keyboardButtons);
                replyKeyboardMarkup.ResizeKeyboard = true;
                replyKeyboardMarkup.OneTimeKeyboard = true;
                replyMarkup = replyKeyboardMarkup;
            }

            //Send message from user
            if (result.IsSendMessage)
            {
                await _bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: result.TextMessage,
                    parseMode:ParseMode.Html,
                    replyMarkup: replyMarkup
                    );
            }

            return Ok();
        }
    }
}
