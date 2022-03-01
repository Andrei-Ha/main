using Exadel.OfficeBooking.TelegramApi.EF;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class SelectBookingType : Step
    {
        public SelectBookingType(HttpClient http, TelegramBot telegramBot, TelegramDbContext context) : base(http, telegramBot, context)
        {}
        public override async Task CurrentStepHandle(Update update)
        {

            await SendKeyboardButtons(update, Array.Empty<string>(), "Select booking type");
            await SetNextStep(update, nameof(SelectBookingType));
        }
    }
}
