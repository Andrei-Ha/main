using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.EF;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class ActionChoice : Step
    {
        public ActionChoice(HttpClient http, TelegramBot telegramBot, TelegramDbContext context)
            : base(http, telegramBot, context) { }

        public override async Task CurrentStepHandle(Update update)
        {
            string? text = update.Message?.Text;

            var cities = await GetSitiesArray();

            if (text == "Book a workplace")
            {
                await SendKeyboardButtons(update, cities, "Choose city");
                await SetNextStep(update, nameof(SelectOffice));
            }

            if (text == "Change or cancel a booking")
            {
                await SendKeyboardButtons(update, Array.Empty<string>(), "Not implemented -((");
                await SetNextStep(update, nameof(Finish));
            }

            if (text == "Nothing")
            {
                await SendKeyboardButtons(update, Array.Empty<string>(), "Bye bye");
                await SetNextStep(update, nameof(Finish));
            }
        }

        private async Task<string[]> GetSitiesArray()
        {
            var offices = await GetRequestServer<OfficeDto[]>("office");

            if (offices != null || offices != Array.Empty<OfficeDto>())
            {
                return offices.Select(x => x.City).Distinct().OrderBy(y => y).ToArray();
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
