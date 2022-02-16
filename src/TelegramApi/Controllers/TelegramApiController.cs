using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello Team3!");
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] object upd)
        {
            var update = JsonConvert.DeserializeObject<Update>(upd.ToString());
            if (update?.Type != UpdateType.Message)
                return NotFound();

            if (update.Message!.Type != MessageType.Text)
                return NotFound(); ;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            messageText = messageText??"no text";
            Message sentMessage = await _telegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                parseMode: ParseMode.Markdown
                );
            return Ok();
        }
    }
}
