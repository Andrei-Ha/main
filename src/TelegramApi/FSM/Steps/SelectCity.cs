using Exadel.OfficeBooking.TelegramApi.DTO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class SelectCity : Step
    {
        public SelectCity(HttpClient http, TelegramBot telegramBot) : base(http, telegramBot)
        {}
        public override async Task CurrentStepHandle(Update update)
        {
            string? text = update.Message?.Text;
            if (text == "Delhi")
            {
                await SendKeyboardButtons(update, new string[] { }, "There are no offices in Delhi");
                await SetNextStep(update, nameof(SelectCity));
            }

            if (text == "Singapore")
            {
                //save user's selected city
                await SendKeyboardButtons(update, new string[] { }, "Bye bye");
                await SetNextStep(update, nameof(Finish));
            }
        }
    }
}
