﻿using Exadel.OfficeBooking.TelegramApi.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Exadel.OfficeBooking.TelegramApi.States.StatesHandlers
{
    public class GreetingsStateHandler
    {
        private readonly TelegramBotClient _botClient;

        private readonly IHttpClientFactory _httpClientFactory;

        public GreetingsStateHandler(TelegramBotClient botClient, IHttpClientFactory httpClientFactory)
        {
            _botClient = botClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StatesNamesEnum> CurrentStateHandle(Update update)
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

                return StatesNamesEnum.SelectCity;
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Something going wrong",
                    parseMode: ParseMode.Markdown);

                return StatesNamesEnum.Greetings;
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
