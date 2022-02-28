using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps;

public class ActionChoice : Step
{
    public ActionChoice(HttpClient http, TelegramBot telegramBot) : base(http, telegramBot) {}

    public override async Task CurrentStepHandle(Update update)
    {
        string? text = update.Message?.Text;
        if (text == "Change or cancel a booking")
        {
            await SendKeyboardButtons(update, new string[] { }, "Next time...");
            await SetNextStep(update, nameof(Finish));
        }

        if (text == "Nothing")
        {
            await SendKeyboardButtons(update, new string[] { }, "Bye bye");
            await SetNextStep(update, nameof(Finish));
        }

        if (text == "Book a workplace")
        {
            await SendKeyboardButtons(update, new string[] { "Delhi", "Singapore" }, "Choose city");
            await SetNextStep(update, nameof(SelectCity));
        }
    }
}
