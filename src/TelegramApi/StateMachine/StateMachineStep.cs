using Exadel.OfficeBooking.TelegramApi.Steps;
using System.IO;
using System.Net.Http;
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

        public FsmState State { get; set; } = null!;

        public abstract FsmState Execute(Update update, FsmState fsmState);

        protected async Task<string> GetJsonFromWebAPI(string controller)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://localhost:7110/api/{controller}");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _http.CreateClient();

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
    }

    public class Result
    {
        public string TextMessage { get; set; } = "Not implemented yet";

        public string NextStep { get; set; } = "Finish";

        public string[] Propositions = System.Array.Empty<string>();
    }
}
