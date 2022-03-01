using Exadel.OfficeBooking.TelegramApi.EF;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public abstract class Step
    {
        private readonly HttpClient _http;
        private readonly TelegramBotClient _client;
        private readonly TelegramDbContext _context;

        protected Step(HttpClient http, TelegramBot telegramBot, TelegramDbContext context)
        {
            _http = http;
            _context = context;
            _client = telegramBot.GetBot().Result;
        }

        public abstract Task CurrentStepHandle(Update update);

        /*
         * All these methods are helper methods
         * You can find documentation for them below
         */
        
        //sets next step for user
        public async Task SetNextStep(Update update, string nextStepName)
        {
            var userState = await _context.UsersStates.AsNoTracking()
                .FirstOrDefaultAsync(u => u.ChatId == update.Message.Chat.Id);

            userState.StepName = nextStepName;

            _context.UsersStates.Update(userState);
            await _context.SaveChangesAsync();
        }

        /*
         * Makes http GET request to server to get data
         * For example:
         * GetRequestServer<GetUserDto[]>("user") => returns array of all users
         * GetRequestServer<WorkplaceGetDto>("workplace/workplace_id") => returns workplace with given id
         */
        public async Task<T?> GetRequestServer<T>(string endpoint)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            HttpResponseMessage response = await _http.GetAsync(uri);
            Stream responseStream = await response.Content.ReadAsStreamAsync();
        
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T? responseObject = await JsonSerializer.DeserializeAsync<T>(responseStream, options);
            return responseObject;
        }

        /*
         * Makes Http POST request to server, where T1 - return type, T2 - type request's body
         * For example:
         * PostRequestServer<GetMapDto, SetMapDto>("map", SetMapDto newMap) => makes post request to
         *                                                                     given endpoint and returns result
         */
        public async Task<T1?> PostRequestServer<T1,T2>(string endpoint, T2 postObj)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            var response = await _http.PostAsJsonAsync(uri, postObj);

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T1? responseObject = await JsonSerializer.DeserializeAsync<T1>(responseStream, options);
            
            return responseObject;
        }

        /*
         * Makes Http PUT request to server, where T1 - return type, T2 - type request's body
         */
        public async Task<T1?> PutRequestServer<T1, T2>(string endpoint, T2 putObj)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            var response = await _http.PutAsJsonAsync(uri, putObj);

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T1? responseObject = await JsonSerializer.DeserializeAsync<T1>(responseStream, options);
            
            return responseObject;
        }
        
        //sends message to client
        public async Task SendMessage(Update update, string text)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: text,
                parseMode: ParseMode.Markdown
            );
        }

        /*
         * Sends keyboard buttons to client with strings inside of buttons array
         */
        public async Task SendKeyboardButtons(Update update, string[] buttons, string message)
        {
            // Create custom keyboard or Remove
            IReplyMarkup replyMarkup;
            if (buttons.Length == 0)
            {
                replyMarkup = new ReplyKeyboardRemove();
            }
            else
            {
                KeyboardButton[][] keyboardButtons = buttons.Select(k => new KeyboardButton[] { k }).ToArray();
                ReplyKeyboardMarkup replyKeyboardMarkup = new(keyboardButtons);
                replyKeyboardMarkup.ResizeKeyboard = true;
                replyMarkup = replyKeyboardMarkup;
            }

            //Send message to user
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: message,
                replyMarkup: replyMarkup
            );
        }
    }
}
