using Exadel.OfficeBooking.TelegramApi.DTO;
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

        private OfficeDto[]? _offices;

        public TelegramApiController(TelegramBot telegramBot, IHttpClientFactory httpClientFactory)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update.Message!.Type != MessageType.Text)
                return Ok();

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            if (messageText == "start")
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7110/api/office");
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _httpClientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    //Users = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<GetUserDto>>(responseStream);

                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();
                    _offices = JsonConvert.DeserializeObject<OfficeDto[]>(text);
                }
                else
                {
                    _offices = Array.Empty<OfficeDto>();
                }

                if (_offices != null)
                {
                    var countries = _offices.Select(x => x.Country).ToArray();

                    var countriesToString = string.Empty;

                    for(int i = 0; i < countries.Length; i++)
                    {
                        if (i == 0)
                            countriesToString += countries[i];
                        
                        else
                            countriesToString += $"\n{countries[i]}";
                    }

                    await _telegramBotClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: countriesToString,
                        parseMode: ParseMode.Markdown);

                    return Ok();
                }
            }

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
