using System;
using System.IO;
using Exadel.OfficeBooking.TelegramApi.FSM;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Mapster;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Exadel.OfficeBooking.TelegramApi.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class TelegramApiController : ControllerBase
    {
        private readonly TelegramBotClient _client;
        private readonly HttpClient _http;

        private readonly StateMachine _stateMachine;

        public TelegramApiController(TelegramBot telegramBot, StateMachine stateMachine, HttpClient http)
        {
            _client = telegramBot.GetBot().Result;
            _stateMachine = stateMachine;
            _http = http;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Get request working");
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Update update)
        {
            if (update.Message!.Type != MessageType.Text)
                return Ok();

            await _stateMachine.IncomingUpdateHandle(update);
            return Ok();
        }
        
        public async Task SetNextStep(long userTelegramId, string nextStepName)
        {
            //find user with given telegramId
            string getEndpoint = $"user/telegram/{userTelegramId}";
            ServiceResponse<GetUserDto>? user = await GetRequestServer<ServiceResponse<GetUserDto>>(getEndpoint);
            
            //change user's state
            string putEndpoint = $"user/{user.Data.Id}";
            SetUserDto updatedUser = user.Data.Adapt<SetUserDto>();
            updatedUser.StepName = nextStepName;
            
            await PutRequestServer<GetUserDto, SetUserDto>(putEndpoint, updatedUser);
        }
        public async Task<T1?> PutRequestServer<T1, T2>(string endpoint, T2 putObj)
        {
            string uri = $"https://localhost:7110/api/{endpoint}";
            var response = await _http.PutAsJsonAsync(uri, putObj);

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            T1? responseObject = await JsonSerializer.DeserializeAsync<T1>(responseStream, options);
            
            return responseObject;
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
