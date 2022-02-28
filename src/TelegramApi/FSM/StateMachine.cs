using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public class StateMachine
    {
        private Step[] _steps;
        private readonly HttpClient _http;
        private readonly IServiceProvider _sp;

        public StateMachine(IServiceProvider sp, HttpClient http)
        {
            _sp = sp;
            _http = http;
            _steps = _sp.GetServices<Step>().ToArray();
        }

        public async Task IncomingUpdateHandle(Update update)
        {
            string curState = await GetOrSetUserState(update);
            
            Step step = _steps.First(step => step.GetType().Name.ToLower() == curState);

            await step.CurrentStepHandle(update);
        }

        private async Task<string> GetOrSetUserState(Update update)
        {
            string endpoint = $"user/telegram/{update.Message.Chat.Id}";
            ServiceResponse<GetUserDto>? user = await GetRequestServer<ServiceResponse<GetUserDto>>(endpoint);

            //user was not initialized
            if (user.Data == null || user.Data.StepName.ToLower() == "finish") return "greetings";

            return user.Data.StepName.ToLower();
        }

        public async Task<T?> GetRequestServer<T>(string endpoint)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            HttpResponseMessage response = await _http.GetAsync(uri);
            Stream responseStream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T? responseObject = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
            return responseObject;
        }
    }
}
