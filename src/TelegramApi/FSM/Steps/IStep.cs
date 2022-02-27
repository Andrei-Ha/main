using Exadel.OfficeBooking.TelegramApi.EF;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Exadel.OfficeBooking.TelegramApi.FSM.Steps
{
    public interface IStep
    {
        Task<UserState> CurrentStepHandle(Update update, UserState state);
    }
}
