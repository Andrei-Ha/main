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
            if (update?.Type != UpdateType.Message)
                return Ok();

            var message = update.Message!;
            if (message.Type != MessageType.Text)
                return Ok();

            Console.WriteLine(message.From.Id);

            // ! This method tell the user that something is happening on the bot's side !
            await _bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            await _fsm.GetState(message.From.Id);
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
                replyMarkup = replyKeyboardMarkup;
            }

            //Send message from user
            await _bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: result.TextMessage,
                replyMarkup: replyMarkup
                );

            return Ok();
        }
    }
}
