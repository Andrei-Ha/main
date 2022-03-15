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
            long chatId;
            bool isMessage = false;
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    {
                        chatId = update.CallbackQuery.Message.Chat.Id;
                        break;
                    }
                case UpdateType.Message:
                    {
                        if (update.Message.Type != MessageType.Text)
                        {
                            return Ok();
                        }

                        isMessage = true;
                        chatId = update.Message.Chat.Id;
                        break;
                    }
                default:
                    {
                        return Ok();
                    }
            }

            Console.WriteLine(chatId);

            // ! This method tell the user that something is happening on the bot's side !
            if (isMessage) 
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
