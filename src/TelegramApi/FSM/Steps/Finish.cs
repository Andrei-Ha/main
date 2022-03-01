using Exadel.OfficeBooking.TelegramApi.EF;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps;

public class Finish : Step
{
    public Finish(HttpClient http, TelegramBot telegramBot, TelegramDbContext context) : base(http, telegramBot, context)
    {
    }

    public override async Task CurrentStepHandle(Update update)
    {
        //anyway i am not executed :((
        await SendMessage(update, "Bye, thanks for choosing our company :)");
    }
}
