using Exadel.OfficeBooking.TelegramApi.FSM.Steps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Exadel.OfficeBooking.TelegramApi.EF;
using System.Linq;

namespace Exadel.OfficeBooking.TelegramApi.FSM
{
    public class StateMachine
    {
        private readonly Step[] _steps;
        private readonly HttpClient _httpClient;
        private UserState _userState = new();
        private IServiceProvider _sp;
        private readonly TelegramDbContext _context;

        public StateMachine(IServiceProvider sp, HttpClient httpClient, TelegramDbContext context)
        {
            _sp = sp;
            _context = context;
            _httpClient = httpClient;
            _steps = _sp.GetServices<Step>().ToArray();

        }

        public async Task IncomingUpdateHandle(Update update)
        {
            string stepName = await GetUserStepName(update);

            Step step = _steps.First(step => step.GetType().Name == stepName);

            await step.CurrentStepHandle(update);
        }

        private async Task<string> GetUserStepName(Update update)
        {
            _userState = await _context.UsersStates.AsNoTracking()
                .FirstOrDefaultAsync(u => u.ChatId == update.Message.Chat.Id);

            if (_userState == null)
            {
                _userState = new UserState { ChatId = update.Message.Chat.Id };
                await _context.UsersStates.AddAsync(_userState);
                await _context.SaveChangesAsync();
            }

            if(_userState.StepName == "Finish") return "Greetings";

            return _userState.StepName;
        }

        public async Task<T?> GetRequestServer<T>(string endpoint)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            Stream responseStream = await response.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T? responseObject = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
            return responseObject;
        }
    }
}
