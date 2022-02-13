using Microsoft.AspNetCore.Mvc;
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
        private readonly TelegramBotClient _telegramBotClient;

        public TelegramBotController(TelegramBot telegramBot)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
        }

        [HttpPost]
        public async Task <IActionResult> Update([FromBody]Update update)
        {
            var chatId = update.Message.Chat.Id;
            var chat = update.Message?.Chat;

            if (chat == null)
            {
                return Ok();
            }
                        
            await _telegramBotClient.SendTextMessageAsync(
                chatId: chatId, 
                text: "Test message", 
                ParseMode.MarkdownV2);

            return Ok();
        }
    }
}
