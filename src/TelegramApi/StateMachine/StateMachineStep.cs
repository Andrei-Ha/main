using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.Steps;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exadel.OfficeBooking.TelegramApi.StateMachine
{
    public abstract class StateMachineStep
    {
        private readonly IHttpClientFactory _http;
        public StateMachineStep(IHttpClientFactory http)
        {
            _http = http;
        }

        public abstract FsmState Execute(Update update, FsmState fsmState);

        protected async Task<string> GetJsonFromWebAPI(string controller)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,controller);
            var client = _http.CreateClient("WebAPI");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                return reader.ReadToEnd();
            }
            else
            {
                return string.Empty;
            }
        }

        protected async Task<LoginUserDto?> Login(long tgId)
        {
            var client = _http.CreateClient("WebAPI");
            using var response = await client.PostAsync("login", new StringContent(tgId.ToString(), Encoding.UTF8,"application/json"));
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                var loginUserDto = JsonConvert.DeserializeObject<LoginUserDto>(reader.ReadToEnd());
                return loginUserDto;
            }
            else
            {
                return null;
            }
        }
    }
}
