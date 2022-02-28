using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps;

public class Finish : Step
{
    public Finish(HttpClient http, TelegramBot telegramBot) : base(http, telegramBot)
    {
    }

    public override async Task CurrentStepHandle(Update update)
    {
        //anyway i am not executed :((
        await SendMessage(update, "Bye, thanks for choosing our company :)");
    }
}
