using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.States;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private readonly IStateMachine _stateMachine;

        private StatesNamesEnum _state;

        private OfficeDto[]? _offices;

        public TelegramApiController(TelegramBot telegramBot, IHttpClientFactory httpClientFactory, IStateMachine stateMachine)
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

            if(messageText == "start")
            {
                _stateMachine.SetState(StatesNamesEnum.Greetings);
            }

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
