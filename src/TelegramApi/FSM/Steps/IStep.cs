using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public interface IStep
    {
        Task<FsmState> CurrentStepHandle(Update update);
    }
}
