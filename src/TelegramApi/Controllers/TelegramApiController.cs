using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exadel.OfficeBooking.TelegramApi.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public TelegramBotController(TelegramBot telegramBot, IHttpClientFactory httpClientFactory)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task <IActionResult> Update([FromBody]Update update)
        {
            var chatId = update.Message?.Chat.Id;
            var chat = update.Message?.Chat;

            if (chat == null)
            {
                return Ok("Chat is empty!");
            }

            await _telegramBotClient.SendTextMessageAsync(
                chatId, 
                text: "Test message.", 
                ParseMode.MarkdownV2,
                disableNotification: false,
                replyToMessageId: update.Message?.MessageId);

            return Ok();
        }
    }
}
