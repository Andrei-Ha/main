using Exadel.OfficeBooking.TelegramApi.FSM;
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
    public class TelegramApiController : ControllerBase
    {
        private readonly TelegramBotClient _client;
        private readonly HttpClient _http;

        private readonly StateMachine _stateMachine;

        public TelegramApiController(TelegramBot telegramBot, StateMachine stateMachine, HttpClient http)
        {
            _client = telegramBot.GetBot().Result;
            _stateMachine = stateMachine;
            _http = http;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Get request working");
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update.Message!.Type != MessageType.Text)
                return Ok();

            await _stateMachine.IncomingUpdateHandle(update);
            //await _client.SendTextMessageAsync(
            //    chatId: update.Message.Chat.Id,
            //    text: update.Message?.Text,
            //    parseMode: ParseMode.Markdown
            //);

            return Ok();
        }
    }
}
