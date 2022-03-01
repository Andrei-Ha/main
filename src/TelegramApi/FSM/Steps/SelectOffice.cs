using Exadel.OfficeBooking.TelegramApi.DTO;
using Exadel.OfficeBooking.TelegramApi.EF;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class SelectOffice : Step
    {
        public SelectOffice(HttpClient http, TelegramBot telegramBot, TelegramDbContext context) : base(http, telegramBot, context)
        {}
        public override async Task CurrentStepHandle(Update update)
        {
            string? messageText = update.Message?.Text;

            var offices = await GetRequestServer<OfficeDto[]>("office");

            var officesInfo = offices
                    .Where(x => x.City == messageText)
                    .Select(o => $"Name: {o.Name}\nAddress: {o.Address}" )
                    .ToArray();

            await SendKeyboardButtons(update, officesInfo, "Choose office");
            await SetNextStep(update, nameof(SelectBookingType));
        }
    }
}
