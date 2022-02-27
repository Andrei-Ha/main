using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.EF;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class Greetings : IStep
    {
        private readonly TelegramBotClient _botClient;

        private readonly IHttpClientFactory _httpClientFactory;

        public Greetings(TelegramBot telegramBot, IHttpClientFactory httpClientFactory)
        {
            _botClient = telegramBot.GetBot().Result;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserState> CurrentStepHandle(Update update, UserState state)
        {
            var chatId = update.Message.Chat.Id;

            var offices = await GetAllOffices();

            if (offices != Array.Empty<OfficeDto>())
            {
                var cities = offices.Select(x => x.City).OrderBy(y => y).ToArray();

                var citiesToString = string.Empty;

                foreach(var city in cities)
                    citiesToString += $"{city}\n";

                citiesToString += "\nSelect and enter the desired city =>";

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: citiesToString,
                    parseMode: ParseMode.Markdown);

                return new UserState { StepName = nameof(SelectCity) };
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Something going wrong",
                    parseMode: ParseMode.Markdown);

                return new UserState { StepName = nameof(Greetings) };
            }
        }

        private async Task<OfficeDto[]?> GetAllOffices()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7110/api/office");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                var reader = new StreamReader(responseStream);

                string text = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<OfficeDto[]>(text);
            }
            else
            {
                return Array.Empty<OfficeDto>();
            }
        }
    }
}
