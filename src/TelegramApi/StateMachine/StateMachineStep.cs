using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Exadel.OfficeBooking.TelegramApi.Steps;
using Newtonsoft.Json;
using System.IO;
using System.Net;
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
        protected FsmState _fsmState = new();
        public StateMachineStep(IHttpClientFactory http)
        {
            _http = http;
        }

        public void TransmitFsmState(FsmState fsmState)
        { 
            _fsmState = fsmState;
        }

        public abstract Task<FsmState> Execute(Update update);

        protected async Task<HttpResponse<T>?> GetModelFromWebAPI<T>(string controller)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,controller);
            request.Headers.Add("Authorization", $"Bearer {_fsmState.User.Token}"); 
            var client = _http.CreateClient("WebAPI");
            var response = await client.SendAsync(request);
            System.Console.WriteLine(response.StatusCode.ToString());
            var httpResponse = new HttpResponse<T>() { StatusCode = response.StatusCode};
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);

                httpResponse.Model = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            return httpResponse;
        }

        protected async Task<bool> Login()
        {
            var client = _http.CreateClient("WebAPI");
            using var response = await client.PostAsync("login", new StringContent(_fsmState.TelegramId.ToString(), Encoding.UTF8,"application/json"));
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                StreamReader reader = new(responseStream);
                var loginUserDto = JsonConvert.DeserializeObject<LoginUserDto>(reader.ReadToEnd());
                if (loginUserDto != null)
                {
                    _fsmState.User = loginUserDto;
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }
    }
}
