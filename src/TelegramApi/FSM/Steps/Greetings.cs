using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.DTO.PersonDto;
using Mapster;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class Greetings : Step
    {
        public Greetings(HttpClient http, TelegramBot telegramBot) : base(http, telegramBot)
        {}
        
        public override async Task CurrentStepHandle(Update update)
        {
            string message = "What do you want to do today";
            string[] buttons = {"Change or cancel a booking", "Book a workplace", "Nothing"};
            
            await SendKeyboardButtons(update, buttons, message);
            await SetNextStep(update, nameof(ActionChoice));
        }
    }
}
