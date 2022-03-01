using Exadel.OfficeBooking.TelegramApi.EF;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class Greetings : Step
    {
        public Greetings(HttpClient http, TelegramBot telegramBot, TelegramDbContext context) : base(http, telegramBot, context)
        {}
        
        public override async Task CurrentStepHandle(Update update)
        {
            string message = "What do you want to do today";
            string[] buttons = { "Change or cancel a booking", "Book a workplace", "Nothing" };
            
            await SendKeyboardButtons(update, buttons, message);
            await SetNextStep(update, nameof(ActionChoice));
        }
    }
}
