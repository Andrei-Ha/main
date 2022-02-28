using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public class SelectOffice : Step
    {
        public SelectOffice(HttpClient http, TelegramBot telegramBot) : base(http, telegramBot)
        {}
        public override Task CurrentStepHandle(Update update)
        {
            throw new System.NotImplementedException();
        }
    }
}
