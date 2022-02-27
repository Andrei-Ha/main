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
        private readonly TelegramBotClient _telegramBotClient;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly StateMachine _stateMachine;

        public TelegramApiController(TelegramBot telegramBot, IHttpClientFactory httpClientFactory, StateMachine stateMachine)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
            _httpClientFactory = httpClientFactory;
            _stateMachine = stateMachine;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update.Message!.Type != MessageType.Text)
                return Ok();


            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            await _stateMachine.IncomingUpdateHandle(update);

            messageText = messageText ?? "no text";
            await _telegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: messageText,
                parseMode: ParseMode.Markdown
                );
            return Ok();
        }
    }
}
