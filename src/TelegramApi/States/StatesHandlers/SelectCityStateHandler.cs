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
    public class SelectCityStateHandler
    {
        private readonly TelegramBotClient _botClient;

        private readonly IHttpClientFactory _httpClientFactory;

        public SelectCityStateHandler(TelegramBotClient botClient, IHttpClientFactory httpClientFactory)
        {
            _botClient = botClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<StatesNamesEnum> CurrentStateHandle(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            var offices = await GetAllOffices();

            if(offices != Array.Empty<OfficeDto>())
            {
                var officesInfo = offices
                    .Where(x => x.City == messageText)
                    .Select(o => new { OfficeName = o.Name, OfficeAddress = o.Address })
                    .ToArray();

                var officesInfoToString = string.Empty;

                foreach(var officeInfo in officesInfo)
                    officesInfoToString += $"Name: {officeInfo.OfficeName}\n" + $"Address: {officeInfo.OfficeAddress}\n\n";

                officesInfoToString += "Select and enter the Name of the desired Office  =>";

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: officesInfoToString,
                    parseMode: ParseMode.Markdown);

                return StatesNamesEnum.SelectOffice;
            }
            else
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Something going wrong",
                    parseMode: ParseMode.Markdown);

                return StatesNamesEnum.SelectCity;
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
